using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public abstract class BaseEntity //we can never create an instance of BaseEntity on its own we can only create a class that implements it
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; } // not required but good practice so when we look into db for troubleshooting we can see when classes were created
        public BaseEntity()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }
    }
}
