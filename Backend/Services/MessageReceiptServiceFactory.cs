using System;

namespace Backend.Services
{
    public class MessageReceiptServiceFactory
    {
        private string tenant;
        
        public void InitializeTenant(string tenant)
        {
            this.tenant = tenant;
        }

        public IReceiptSender CreateSender()
        {
            switch (tenant.ToLower())
            {
                case "shoestore": return new EmailReceiptSender();
                case "watchstore": return new TextMessageReceiptSender();
            }
            
            throw new InvalidOperationException($"Tenant {tenant} need to specify email sender.");
        }
    }
}