using System.Threading.Tasks;
using Backend.Services;
using Messages;
using NServiceBus;

namespace Backend
{
    public class CustomerReceiptSenderHandler : IHandleMessages<SendCustomerReceipt>
    {
        private readonly MessageReceiptServiceFactory receiptServiceFactory;

        public CustomerReceiptSenderHandler(MessageReceiptServiceFactory receiptServiceFactory)
        {
            this.receiptServiceFactory = receiptServiceFactory;
        }
        
        public Task Handle(SendCustomerReceipt message, IMessageHandlerContext context)
        {
            var service = receiptServiceFactory.CreateSender();
            
            return service.SendReceipt(message.Customer);
        }
    }
}