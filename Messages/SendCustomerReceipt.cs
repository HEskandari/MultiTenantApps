using NServiceBus;

namespace Messages
{
    public class SendCustomerReceipt : ICommand
    {
        public string Customer { get; set; }
    }
}