using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;  // added this using statement
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext  // inherit from DbContext
    {
        public DataContext()  // constructor so it can capture and pass in the connection string the base class expects
            : base("DefaultConnection") // causes the Dbcontext class to look into our web.config and looks for "DefaultConnection"
        {

        }

        //Tells explicitly DbContext which Models to be stored in Tables
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
