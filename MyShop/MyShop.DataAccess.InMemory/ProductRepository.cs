using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;using System.Runtime.Caching;using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)   // if no products found in cache 
            {
                products = new List<Product>(); // it will create a new list of products
            }
        }

        public void Commit()
        { // so it doesnt save products straight to cache will need to be explicitly saved
            cache["products"] = products;
        }

        public void Insert(Product p)
        {
            products.Add(p);
        }

        public void Update(Product product)
        {
            Product productToUpDate = products.Find(p => p.Id == product.Id);

            if (productToUpDate != null)
            {
                productToUpDate = product;
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string Id)
        {
            Product productToDelete = products.Find(p => p.Id == Id);

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }
    }}
