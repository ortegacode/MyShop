using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class BasketItem : BaseEntity
    {
        public string BasketId { get; set; } // link back to the basket that contains the basket items
        public string ProductId { get; set; }  // link to product
        public int Quantity { get; set; }
    }
}
