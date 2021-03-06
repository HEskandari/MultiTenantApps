using NServiceBus;

namespace Messages.Extensions
{
    public static class MessageRoutingExtensions
    {
        public static void ApplyDefaultRouting(this TransportExtensions transport)
        {
            var routing = transport.Routing();
            
            routing.RouteToEndpoint(typeof(CreateOrderCommand), "Backend");
            routing.RouteToEndpoint(typeof(SendCustomerReceipt), "Backend");
        }
    }
}