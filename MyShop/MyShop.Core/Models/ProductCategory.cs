using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory : BaseEntity
    {
        //public string Id { get; set; }  // fields done in BaseEntity
        public string Category { get; set; }

        //public ProductCategory() //done in BaseEntity
        //{
        //    this.Id = Guid.NewGuid().ToString(); // constructor
        //}
    }
}
