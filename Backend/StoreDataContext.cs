using System.Data.Common;
using Backend.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class StoreDataContext : DbContext
    {
        DbConnection connection;

        public StoreDataContext()
        {
        }
        
        public StoreDataContext(DbConnection connection)
        {
            this.connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connection);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var orders = modelBuilder.Entity<Order>();
            orders.ToTable("Orders");
            orders.HasKey(x => x.OrderId);
            orders.Property(x => x.Quantity);
            orders.HasOne(x => x.Product);
        }

        public static void GenerateDatabase(string connectionString, string tenantDbName)
        {
            var tenantConnection = connectionString.Replace("[TenantDB]", tenantDbName);
            using (var connection = new SqlConnection(tenantConnection))
            using (var dbcontext = new StoreDataContext(connection))
            {
                dbcontext.Database.EnsureCreated();
            }
        }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}