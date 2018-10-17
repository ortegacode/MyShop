using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using MyShop.Core.ViewModels;
using MyShop.Core.Contracts;
using System.IO; // for Path

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        //ProductRepository context;   // instance of our product repository
        //ProductCategoryRepository productCategories; // so we can load productCategories from database
        // replaced by the generic repository below

        //InMemoryrepository<Product> context; not used anymore we r using Irepository down below
        //InMemoryrepository<ProductCategory> productCategories;

        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext) //injected the actual classes thru the constructor by accepting the two interfaces
        {
            //context = new InMemoryrepository<Product>(); // initialized our product 
            //productCategories = new InMemoryrepository<ProductCategory>();  //initialized productCategories

            context = productContext; // replaced after injection 
            productCategories = productCategoryContext;  
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();  // returns a list of products from collection
            return View(products);
        }

        public ActionResult Create()   //create method for product
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel(); // reference to ProductManagerViewModel
             
            viewModel.Product = new Product(); // empty product  
            viewModel.ProductCategories = productCategories.Collection(); // send in categories from the database
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)  // if our validation is not valid
            {
                return View(product);
            }
            else
            {
                if (file != null) // to make sure a file actually exists because its possible for them to create or save a product without an image
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName); // if there is is an image we set the image property on the product itself using the product.id because a user can upload two files with the same name so by renaming it to the product.id we will always have a unique file reference.   + 
                      // Path.GetExtension() allows us to get the current file extension

                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);  // save to the disk (file.SaveAs) used a special function called Server.MapPath which allows us to enter the relative or virtual path  to the corresponding physical directory on the server ("//Context//ProductImages//").

                }
                context.Insert(product);    // inserts into the collection 
                context.Commit();           // saves the changes using the commit()

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);  //finds product by Id
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();

                return View(viewModel);
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            Product productToEdit = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                    return View(product);
                {
                    if (file != null)
                    {
                        productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                    }

                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    context.Commit();

                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");

            }
        }
    }
}

