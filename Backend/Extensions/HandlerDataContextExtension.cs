using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;

namespace Backend
{
    // public static class HandlerDataContextExtension
    // {
    //     public static Task WithDataContext(this IMessageHandlerContext context, Action<StoreDataContext> action)
    //     {
    //         var session = context.SynchronizedStorageSession.SqlPersistenceSession();
    //
    //         var dataContext = new StoreDataContext(session.Connection);
    //         dataContext.Database.UseTransaction(session.Transaction);
    //         
    //         action(dataContext);
    //         
    //         return dataContext.SaveChangesAsync();
    //     }
    // }
}