using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;using System.Runtime.Caching;using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)   // if no products found in cache 
            {
                productCategories = new List<ProductCategory>(); // it will create a new list of products
            }
        }

        public void Commit()
        { // so it doesnt save products straight to cache will need to be explicitly saved
            cache["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        public void Update(ProductCategory productCategory)
        {
            ProductCategory productCategoryToUpDate = productCategories.Find(p => p.Id == productCategory.Id);

            if (productCategoryToUpDate != null)
            {
                productCategoryToUpDate = productCategory;
            }
            else
            {
                throw new Exception("Product Category not found.");
            }
        }

        public ProductCategory Find(string Id)
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == Id);

            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product Category not found.");
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(p => p.Id == Id);

            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("Product Category not found.");
            }
        }
    }}
