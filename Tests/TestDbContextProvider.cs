using Backend;
using Microsoft.EntityFrameworkCore;
using NServiceBus;

namespace Tests
{
    public class TestDbContextProvider : IDataContextProvider<StoreDataContext>
    {
        public TestDbContextProvider()
        {
            var builder = new DbContextOptionsBuilder<StoreDataContext>();
            builder.UseInMemoryDatabase("TestDb");
            var options = builder.Options;
            DataContext = new StoreDataContext(options);
        }

        public StoreDataContext DataContext { get; }

        public StoreDataContext GetDataContext(IMessageHandlerContext session)
        {
            return DataContext;
        }
    }
}