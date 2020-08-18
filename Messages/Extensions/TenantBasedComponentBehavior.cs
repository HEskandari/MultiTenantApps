using System;
using System.Threading.Tasks;
using Messages.TenantComponents;
using NServiceBus.Pipeline;

namespace Messages.Extensions
{
    public class TenantBasedComponentBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            var tenant = context.MessageHeaders[HeaderKeys.StoreId];
            var factory = context.Builder.Build<TenantMessagingFactory>();

            factory.InitializeTenant(tenant);

            await next();
        }
    }
}