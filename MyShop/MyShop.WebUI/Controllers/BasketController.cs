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
        IBasketService basketService;
        IOrderService orderService; // hooked up to controller

        public BasketController(IBasketService BasketService, IOrderService OrderService)
        {
            this.basketService = BasketService;
            this.orderService = OrderService;
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

        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(Order order)
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext); // get basket items from basketService
            order.OrderStatus = "Order Created";

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