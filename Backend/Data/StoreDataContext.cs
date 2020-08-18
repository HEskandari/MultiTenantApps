using System.Data.Common;
using Backend.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class StoreDataContext : DbContext
    {
        public StoreDataContext(DbContextOptions<StoreDataContext> options) : base(options)
        {
        }
        
        public StoreDataContext(DbConnection connection) : base(CreateDbOptions(connection))
        {
        }

        private static DbContextOptions CreateDbOptions(DbConnection connection)
        {
            var builder = new DbContextOptionsBuilder<StoreDataContext>();
            builder.UseSqlServer(connection);
            return builder.Options;
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