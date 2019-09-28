using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using System.Linq;
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
            //modelBuilder.Entity<>(entity =>
            //{
            //    entity.ToTable("");

            //    entity.HasKey(x =>);

            //    entity.Property(x =>).HasColumnName("");
            //});
        }
    }
}
