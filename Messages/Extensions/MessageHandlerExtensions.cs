using NServiceBus;

namespace Messages
{
    public static class MessageHandlerExtensions
    {
        public static string GetStoreId(this IMessageHandlerContext context)
        {
            var tenant = context.MessageHeaders[HeaderKeys.StoreId];
            return tenant;
        }
    }
}