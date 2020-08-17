using System;
using System.Threading.Tasks;

namespace Backend.Services
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