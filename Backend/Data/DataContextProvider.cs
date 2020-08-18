using System;
using Microsoft.EntityFrameworkCore;
using NServiceBus;

namespace Backend
{
    public interface IDataContextProvider<out T> where T : DbContext
    {
        T GetDataContext(IMessageHandlerContext session);
    }
    
    public class DbContextProvider<T> : IDataContextProvider<T> where T : DbContext
    {
        public T GetDataContext(IMessageHandlerContext context)
        {
            if (context.Extensions.TryGet(out T dataContext))
            {
                return dataContext;
            }
            
            throw new Exception($"DataContext '{typeof(T)} was not set.");
        }
    }
}