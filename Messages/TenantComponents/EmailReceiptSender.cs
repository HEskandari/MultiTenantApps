using System;
using System.Threading.Tasks;

namespace Messages.TenantComponents
{
    public class EmailReceiptSender : IReceiptSender
    {
        public Task SendReceipt(string customer)
        {
            Console.WriteLine("Sending email message for " + customer);
            return Task.CompletedTask;
        }
    }
}