using System.Net;
using System.Text;
using API.Infrastructure;
using API.Infrastructure.Email;
using API.Infrastructure.Jwt;
using API.Jobs;
using DAO.Contexts;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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


        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            // Uptime Service
            services.AddSingleton(new UptimeService());

            // Cors
            services.AddCors();

            // Jwt
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

            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    //opts.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                });
        }

        private void ConfigureServicesJwtAuthentication()
        {
            IConfigurationSection tokenConfigurationSection = _configuration.GetSection("JwtOptions");
            TokenManagement tokenManagement = tokenConfigurationSection.Get<TokenManagement>();

            byte[] securityKey = Encoding.UTF8.GetBytes(tokenManagement.SecurityKey);

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            _services.Configure<TokenManagement>(tokenConfigurationSection);

            _services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameters;
                });
        }

        private void ConfigureServicesIdentity()
        {
            _services.TryAddScoped<SignInManager<User>, SignInManager<User>>();
            _services.TryAddScoped<RoleManager<IdentityRole>, AspNetRoleManager<IdentityRole>>();
            _services.TryAddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();

            IdentityBuilder identityBuilder;
            identityBuilder = _services.AddIdentityCore<User>(x =>
            {
                // configure identity options
                x.Password.RequireDigit = true;
                x.Password.RequireLowercase = true;
                x.Password.RequireUppercase = true;
                x.Password.RequireNonAlphanumeric = true;
                x.Password.RequiredLength = 8;
            });

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), identityBuilder.Services);

            identityBuilder.AddEntityFrameworkStores<DbContextIdentity>().AddDefaultTokenProviders();
        }

        private void ConfigureServicesDbContexts()
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];

            _services.AddDbContext<DbContextIdentity>(options => options.UseMySQL(connectionString));
            _services.AddDbContext<DbContextMain>(options => options.UseMySQL(connectionString));
        }

        private void ConfigureServicesScheduledJobs()
        {
            _services.AddSingleton<IJobFactory, JobFactory>();
            _services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            //_services.AddSingleton<>();
            //_services.AddSingleton(new JobSchedule(
            //    jobType: typeof(),
            //    cronExpression: _configuration["Jobs:<taskname>:Schedule"])
            //);

            _services.AddHostedService<QuartzHostedService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                ConfigureExceptions(app);
            }

            app.UseCors(builder => builder.AllowAnyOrigin());

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
                        //Message = exception.Error.Message,
                        //Message = 
                        //    $"{exception.Error.Message}\r\n" +
                        //    $"{exception.Error.InnerException?.Message}\r\n" +
                        //    $"{exception.Error.InnerException?.InnerException?.Message}",
                        Message = "Internal Server Error."
                    }); ;

                    await context.Response.WriteAsync(error);
                }
            }));
        }
    }
}
