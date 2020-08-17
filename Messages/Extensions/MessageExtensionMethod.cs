using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Frontend
{
    public static class MessageExtensionMethod
    {
        public static Task SendForTenant<T>(this IMessageSession session, string store, T message)
        {
            var sendOption = new SendOptions();
            sendOption.SetHeader(HeaderKeys.StoreId, store);
            return session.Send(message, sendOption);
        }
        
        public static Task SendForTenant<T>(this IMessageHandlerContext session, string store, T message)
        {
            var sendOption = new SendOptions();
            sendOption.SetHeader(HeaderKeys.StoreId, store);
            return session.Send(message, sendOption);
        }
    }
}