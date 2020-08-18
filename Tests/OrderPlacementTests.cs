using System.Threading.Tasks;
using Backend;
using Backend.Model;
using Messages;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Tests
{
    public class OrderPlacementTests
    {
        [Test]
        public async Task CustomerReceiptIsSentWhenOrderIsPlaced()
        {
            var store = "WatchStore";

            var dataProvider = GetTestDataContextProvider();
            var handlerContext = GetTestContextWithHeader(HeaderKeys.StoreId, store);

            var handler = new OrderPlacementHandler(dataProvider);
            var command = new CreateOrderCommand
            {
                ProductID = 10,
                Quantity = 1,
                Customer = "someone@somewhere.com"
            };
            
            await handler.Handle(command, handlerContext);

            Assert.AreEqual(1, dataProvider.DataContext.Orders.Local.Count);
            Assert.AreEqual(1, handlerContext.SentMessages.Length);
            Assert.IsInstanceOf<SendCustomerReceipt>(handlerContext.SentMessages[0].Message);
        }
        
        
        
        private TestableMessageHandlerContext GetTestContextWithHeader(string key, string value)
        {
            var context = new TestableMessageHandlerContext();
            context.MessageHeaders.Add(key, value);

            return context;
        }

        private TestDbContextProvider GetTestDataContextProvider()
        {
            var dbProvider = new TestDbContextProvider();
            dbProvider.DataContext.Products.Add(new Product {ProductId = 10, Name = "Rolex Watch"});

            return dbProvider;
        }
    }
}