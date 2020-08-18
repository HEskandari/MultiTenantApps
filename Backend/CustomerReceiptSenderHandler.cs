using System.Threading.Tasks;
using Messages;
using Messages.TenantComponents;
using NServiceBus;

namespace Backend
{
    public class CustomerReceiptSenderHandler : IHandleMessages<SendCustomerReceipt>
    {
        private readonly TenantMessagingFactory receiptFactory;

        public CustomerReceiptSenderHandler(TenantMessagingFactory receiptFactory)
        {
            this.receiptFactory = receiptFactory;
        }
        
        public Task Handle(SendCustomerReceipt message, IMessageHandlerContext context)
        {
            var service = receiptFactory.CreateSender();
            
            return service.SendReceipt(message.Customer);
        }
    }
}