using MyShop.Core.Contracts; //added
using MyShop.Core.Models; // added
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context;  //copied from ProductManager Controller
        IRepository<ProductCategory> productCategories;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext) 
        {
            context = productContext;  
            productCategories = productCategoryContext;
        }   // up to here

        public ActionResult Index(string Category=null) //updated action for our ProductListViewModel
        {
            //List<Product> products = context.Collection().ToList(); //gets a list of products and sends it to the view
            //changed after ProductListViewModel

            List<Product> products;
            List<ProductCategory> categories = productCategories.Collection().ToList();

            if (Category == null)
            {
                products = context.Collection().ToList(); //if Category is null our list of products will be all products
            }
            else
            {
                products = context.Collection().ToList().Where(p => p.Category == Category).ToList();
                // if not null it will return a list of filtered products using this SQL statement
            }

            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = categories;

            return View(model); //replaced products because we want it to return the model now
        }

        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}