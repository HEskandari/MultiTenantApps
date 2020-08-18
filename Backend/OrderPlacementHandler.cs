using System;
using System.Threading.Tasks;
using Backend.Model;
using Messages;
using NServiceBus;

namespace Backend
{
    public class OrderPlacementHandler : IHandleMessages<CreateOrderCommand>
    {
        private readonly IDataContextProvider<StoreDataContext> dataContextProvider;
        
        public OrderPlacementHandler(IDataContextProvider<StoreDataContext> dataContextProvider)
        {
            this.dataContextProvider = dataContextProvider;
        }
        
        public async Task Handle(CreateOrderCommand message, IMessageHandlerContext context)
        {
            var dbContext = dataContextProvider.GetDataContext(context);
            var product = await dbContext.Products.FindAsync(message.ProductID);
            
            var order = new Order
            {
                ProductId = message.ProductID,
                Quantity = message.Quantity
            };

            await dbContext.Orders.AddAsync(order);
            
            Console.WriteLine($"Order command for (Product={product.Name}) stored.");

            await context.Send(new SendCustomerReceipt
            {
                Customer = message.Customer
            });
        }
    }
}