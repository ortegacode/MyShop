using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Basket : BaseEntity
    {
        public virtual ICollection<BasketItem> BasketItems { get; set; }

        // setting it to virtual ICollection EF will know that every time we load Basket from the db it will also load all the basket items. AKA (Lazy Loading)

        public Basket()
        {
            this.BasketItems = new List<BasketItem>(); // constructor that creates an empty list of basket items on creation
        }
    }
}
