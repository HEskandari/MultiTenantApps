using Microsoft.EntityFrameworkCore;
using NServiceBus;

namespace Backend
{
    public static class EndpointConfigurationExtensions
    {
        public static void UseDataContextUnitOfWork<T>(this EndpointConfiguration endpointConfiguration) where T : DbContext
        {
            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register(new DataContextUnitOfWorkBehavior<T>(), $"Unit of work for EntityFramework data context");
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<DbContextProvider<T>>(DependencyLifecycle.SingleInstance));
        }
    }
}