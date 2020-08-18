using System.Threading.Tasks;
using NServiceBus;

namespace Messages.Extensions
{
    public static class MessageExtensionMethod
    {
        public static Task SendForTenant<T>(this IMessageSession session, string store, T message)
        {
            var sendOption = new SendOptions();
            sendOption.SetHeader(HeaderKeys.StoreId, store);
            return session.Send(message, sendOption);
        }
    }
}