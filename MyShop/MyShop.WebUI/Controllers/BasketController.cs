using MyShop.Core.Contracts; // added
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IRepository<Customer> customers;
        IBasketService basketService;
        IOrderService orderService; // hooked up to controller

        public BasketController(IBasketService BasketService, IOrderService OrderService, IRepository<Customer> Customers)
        {
            this.basketService = BasketService;
            this.orderService = OrderService;
            this.customers = Customers;
        }

        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext); // returning our basket view items
            return View(model);
        }

        public ActionResult AddToBasket(string Id)   // adds to basket
        {
            basketService.AddToBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)   // removes from basket
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }

        [Authorize]  // makes sure user is logged in
        public ActionResult Checkout()
        {
            Customer customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);
            // retrieved customer from the db based on the login email info using Identity

            if (customer != null) // make sure customer is not null and has a customer record
            {
                Order order = new Order() // create new order and prefill users details
                {
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    Street = customer.Street,
                    FirstName = customer.FirstName,
                    SurName = customer.LastName,
                    Zipcode = customer.Zipcode
                };
                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order)
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext); // get basket items from basketService
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name; // make sure user is logged in and that we r linking the login email with the actual order

            //process payment

            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);  // created our order with the basket items
            basketService.ClearBasket(this.HttpContext);  // clear the basket

            return RedirectToAction("ThankYou", new { OrderId = order.Id });  // sent them to our thank u page with an order id
        }

        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
       
    }
}