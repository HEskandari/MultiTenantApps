using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int OrderId { get; set; }
        public virtual int Quantity { get; set; }
        
        public virtual Product Product { get; set; }
        public virtual int ProductId { get; set; }
    }
}