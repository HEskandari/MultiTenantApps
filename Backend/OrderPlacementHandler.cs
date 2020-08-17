using System;
using System.Threading.Tasks;
using Backend.Model;
using Frontend;
using Messages;
using NServiceBus;

namespace Backend
{
    public class OrderPlacementHandler : IHandleMessages<CreateOrderCommand>
    {
        public async Task Handle(CreateOrderCommand message, IMessageHandlerContext context)
        {
            Product product = default;
            
            await context.WithDataContext(db =>
            {
                product = db.Products.Find(message.ProductID);
                
                var order = new Order
                {
                    ProductId = message.ProductID,
                    Quantity = message.Quantity
                };

                db.Orders.Add(order);
            });
            
            var store = context.GetStoreId();
            Console.WriteLine($"Order command for {store} tenant stored (Product={product.Name}).");

            await context.SendForTenant(store, new SendCustomerReceipt
            {
                Customer = message.Customer
            });
        }
    }
}