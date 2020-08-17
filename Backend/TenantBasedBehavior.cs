using System;
using System.Threading.Tasks;
using Backend.Services;
using Messages;
using NServiceBus.Pipeline;

namespace Backend
{
    public class TenantBasedBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            var tenant = context.MessageHeaders[HeaderKeys.StoreId];
            var factory = context.Builder.Build<MessageReceiptServiceFactory>();

            factory.InitializeTenant(tenant);

            await next();
        }
    }
}