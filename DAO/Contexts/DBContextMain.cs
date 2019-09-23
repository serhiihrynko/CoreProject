using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAO.Contexts
{
    public class DbContextMain : DbContextBase
    {
        public DbContextMain(DbContextOptions options) : base(options) { }

        public DbContextMain(string connectionString) : base(connectionString) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureModel(ModelBuilder modelBuilder)
        {
        }
    }
}
