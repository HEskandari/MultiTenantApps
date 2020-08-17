using System.Data.SqlClient;
using Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);

            builder.UseNServiceBus(context =>
            {
                var connectionString = context.Configuration["TransportConnectionString"];
                var dbConnection = context.Configuration["DBConnectionString"];
                var config = new EndpointConfiguration("Frontend");

                config.EnableInstallers();
                config.SendFailedMessagesTo("error");
                
                var transport = config.UseTransport<RabbitMQTransport>();
                transport.ConnectionString(connectionString);
                transport.UseDirectRoutingTopology();
                transport.ApplyDefaultRouting();
                
                var persistence = config.UsePersistence<SqlPersistence>();
                persistence.SqlDialect<SqlDialect.MsSqlServer>();
                persistence.MultiTenantConnectionBuilder(tenantIdHeaderName: HeaderKeys.StoreId, tenantId =>
                {
                    var tenantConnection = dbConnection.Replace("[TenantDB]", $"MultiTenantApp_{tenantId}");
                    return new Microsoft.Data.SqlClient.SqlConnection(tenantConnection);
                });
                
                return config;
            });
            
            builder.Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(k => k.ListenAnyIP(5001));
                    webBuilder.UseUrls("http://*:5001");
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}