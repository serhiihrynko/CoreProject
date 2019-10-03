using System;
using System.Net;
using System.Text;
using API.Infrastructure;
using API.Infrastructure.Email;
using API.Infrastructure.Jwt;
using API.Infrastructure.Scheduler;
using DAO.Contexts;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace API
{
    public class Startup
    {
        private IServiceCollection _services;
        private readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            services.AddSingleton(new UptimeService());

            // Cors
            services.AddCors();

            // JWT
            ConfigureServicesJwtAuthentication();

            // Identity
            ConfigureServicesIdentity();

            // Contexts
            ConfigureServicesDbContexts();

            // Jobs
            ConfigureServicesScheduledJobs();

            // Email Service
            services.Configure<EmailConfig>(_configuration.GetSection("Email"));
            services.AddTransient<IEmailService, EmailService>();

            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                ConfigureExceptions(app);
                app.UseHsts();
            }

            //IdentityInitializer.Initialize(
            //    app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider,
            //    _configuration.GetSection("Identity:User").Get<CreateUserModel>()
            //);

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );

            app.UseAuthentication();

            app.UseMvc();
        }

        private void ConfigureExceptions(IApplicationBuilder app)
        {
            app.UseExceptionHandler(options => options.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var exception = context.Features.Get<IExceptionHandlerFeature>();

                if (exception != null)
                {
                    var error = JsonConvert.SerializeObject(new
                    {
                        Message = "Internal Server Error.",
                        context.Response.StatusCode
                    });

                    await context.Response.WriteAsync(error);
                }
            }));
        }

        private void ConfigureServicesJwtAuthentication()
        {
            var tokenConfigurationSection = _configuration.GetSection("JwtOptions");
            var tokenOptions = tokenConfigurationSection.Get<JwtOptions>();

            _services.Configure<JwtOptions>(tokenConfigurationSection);
            _services.AddSingleton<IJwtFactory, JwtFactory>();

            var securityKey = Encoding.UTF8.GetBytes(tokenOptions.SecurityKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            _services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameters;
                });
        }

        private void ConfigureServicesIdentity()
        {
            _services
                .AddIdentityCore<User>(x =>
                {
                    // Configure Identity password options
                    x.Password.RequireDigit = true;
                    x.Password.RequireLowercase = true;
                    x.Password.RequireUppercase = true;
                    x.Password.RequireNonAlphanumeric = true;
                    x.Password.RequiredLength = 8;
                })
                .AddEntityFrameworkStores<DbContextIdentity>()
                .AddDefaultTokenProviders();
        }

        private void ConfigureServicesDbContexts()
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];

            _services.AddDbContext<DbContextIdentity>(options => options.UseSqlServer(connectionString));
            _services.AddDbContext<DbContextMain>(options => options.UseSqlServer(connectionString));
        }

        private void ConfigureServicesScheduledJobs()
        {
            _services.AddSingleton<IJobFactory, JobFactory>();
            _services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            //_services.AddSingleton<job_class>();
            //_services.AddSingleton(new JobSchedule(
            //    jobType: typeof(job_class),
            //    cronExpression: _configuration["Jobs:<job_name>:Schedule"])
            //);

            _services.AddHostedService<QuartzHostedService>();
        }
    }
}
