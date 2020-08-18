using Messages;
using Messages.Extensions;
using Messages.TenantComponents;
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
                config.UseDataContextUnitOfWork<StoreDataContext>();
                config.AutoFlowTenantInformation();

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
                
                return config;
            });

            builder.ConfigureServices(svc =>
            {
                svc.AddSingleton<TenantMessagingFactory>();
            });
            
            builder.Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args);
        }
    }
}