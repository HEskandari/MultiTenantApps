using System;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class TextMessageReceiptSender : IReceiptSender
    {
        public Task SendReceipt(string customer)
        {
            Console.WriteLine("Sending Text message for " + customer);
            return Task.CompletedTask;
        }
    }
}