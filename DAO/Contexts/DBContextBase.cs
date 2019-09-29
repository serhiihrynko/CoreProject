using Microsoft.EntityFrameworkCore;

namespace DAO.Contexts
{
    public abstract class DbContextBase : DbContext
    {
        private readonly string _connectionString;


        protected DbContextBase(DbContextOptions options) : base(options) { }

        protected DbContextBase(string connectionString)
        {
            _connectionString = connectionString;
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureModel(modelBuilder);
        }

        protected abstract void ConfigureModel(ModelBuilder modelBuilder);
    }
}
