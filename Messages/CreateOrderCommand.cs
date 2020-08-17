using NServiceBus;

namespace Messages
{
    public class CreateOrderCommand : ICommand
    {
        public string Customer { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}