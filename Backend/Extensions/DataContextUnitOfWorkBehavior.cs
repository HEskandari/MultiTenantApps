using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Pipeline;

namespace Backend
{
    public class DataContextUnitOfWorkBehavior<TDataContext> : Behavior<IInvokeHandlerContext> where TDataContext : DbContext
    {
        public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
        {
            var session = context.SynchronizedStorageSession;
            var sqlPersistenceSession = session.SqlPersistenceSession();
            var dbContext = (TDataContext)Activator.CreateInstance(typeof(TDataContext), sqlPersistenceSession.Connection);

            await using (dbContext)
            {
                dbContext.Database.UseTransaction(sqlPersistenceSession.Transaction);
                
                context.Extensions.Set(dbContext);

                await next();

                await dbContext.SaveChangesAsync();
            }
        }
    }
}