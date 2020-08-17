using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IReceiptSender
    {
        Task SendReceipt(string customer);
    }
}