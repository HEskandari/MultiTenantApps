using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

namespace Messages.Extensions
{
    public class FlowTenantInfoBehavior : Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            if (context.MessageHeaders.TryGetValue(HeaderKeys.StoreId, out var tenant))
            {
                context.Extensions.Set(HeaderKeys.StoreId, tenant);
            }

            return next();
        }
    }
    
    public class PropagateTenantInfoBehavior : Behavior<IOutgoingLogicalMessageContext>
    {
        public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            if (context.Extensions.TryGet(HeaderKeys.StoreId, out string tenant))
            {
                context.Headers[HeaderKeys.StoreId] = tenant;
            }

            return next();
        }
    }
    
    public static class EndpointConfigurationExtensions
    {
        public static void AutoFlowTenantInformation(this EndpointConfiguration endpointConfiguration)
        {
            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register(new FlowTenantInfoBehavior(), "Stores Store ID in the session");
            pipeline.Register(new PropagateTenantInfoBehavior(), "Propagates Store ID back to the outgoing messages");
            pipeline.Register(new TenantBasedComponentBehavior(), "Initializes the tenant factory.");
        }
    }
}