using System;
using System.Threading.Tasks;
using Frontend.Model;
using Messages;
using Messages.Extensions;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Frontend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMessageSession session;

        public OrderController(IMessageSession session)
        {
            this.session = session;
        }

        [HttpPost("{store}/create")]
        public async Task<IActionResult> Create(OrderInfo orderInfo, string store)
        {
            #region Sending via Message Body
            /*
            //Don't do this:
            await session.Send(new CreateOrderCommand
            {
                CustomerID = customerId,
                ProductID = orderInfo.ProductID,
                Quantity = orderInfo.Quantity
            });
            */
            #endregion
            
            await session.SendForTenant(store, new CreateOrderCommand
            {
                Customer = orderInfo.Customer,
                ProductID = orderInfo.ProductID,
                Quantity = orderInfo.Quantity
            });
            
            Console.WriteLine($"Sent the order request for {store}");
            
            return Ok($"{store}-OrderAccepted");
        }
    }
}