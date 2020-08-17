using Backend.Services;
using Frontend;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);

            builder.UseNServiceBus(context =>
            {
                var connectionString = context.Configuration["TransportConnectionString"];
                var dbConnection = context.Configuration["DBConnectionString"];
                var config = new EndpointConfiguration("Backend");

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
                
                #region Generate EF Database
                //StoreDataContext.GenerateDatabase(dbConnection, "MultiTenantApp_100");
                //StoreDataContext.GenerateDatabase(dbConnection, "MultiTenantApp_200");
                #endregion
                
                #region Setup Pipeline 
                config.Pipeline.Register(new TenantBasedBehavior(),  "Initializes the tenant factory.");
                #endregion
                
                return config;
            });

            builder.ConfigureServices(svc =>
            {
                svc.AddSingleton<MessageReceiptServiceFactory>();
            });
            
            builder.Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args);
        }
    }
}