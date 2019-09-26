using DAO.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO.Contexts
{
    public abstract class DbContextBase : DbContext
    {
        private string _connectionString;


        public DbContextBase(DbContextOptions options) : base(options) { }

        public DbContextBase(string connectionString)
        {
            _connectionString = connectionString;
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureModelBuilder.OnModelCreating(modelBuilder);

            ConfigureModel(modelBuilder);
        }

        protected abstract void ConfigureModel(ModelBuilder modelBuilder);
    }
}
