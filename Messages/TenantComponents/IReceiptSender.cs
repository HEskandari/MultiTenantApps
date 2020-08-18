using System.Threading.Tasks;

namespace Messages.TenantComponents
{
    public interface IReceiptSender
    {
        Task SendReceipt(string customer);
    }
}