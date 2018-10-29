using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.WebUI.Tests.Mocks;
using MyShop.Services;
using System.Linq;
using MyShop.WebUI.Controllers;
using System.Web.Mvc;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllersTests
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //setup
            IRepository<Basket> baskets = new MockContext<Basket>(); // created the mock repositories
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();

            var httpContext = new MockHttpContext(); //created our httpContext

            IBasketService basketService = new BasketService(products, baskets);  // ceated our basketService which takes our products, baskets context

            IOrderService orderService = new OrderService(orders);

            //// for testing the BasketService
            //basketService.AddToBasket(httpContext, "1"); // to test the service directly

            //Basket basket = baskets.Collection().FirstOrDefault(); // checks if theres any baskets in our basket collection

            //Assert.IsNotNull(basket); // checks basket to make sure it is not null
            //Assert.AreEqual(1, basket.BasketItems.Count); // checks if there is one item in the basket
            //Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId); // checks to make sure the productId in the basket is the one we just injected


            // for testing the BasketController

            //act
            var controller = new BasketController(basketService, orderService);

            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            // how we inject the httpContext into the controller

            controller.AddToBasket("1");

            Basket basket = baskets.Collection().FirstOrDefault();


            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            IRepository<Basket> baskets = new MockContext<Basket>(); 
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();

            products.Insert(new Product() { Id = "1", Price = 10.00m }); // added our products to test
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            Basket basket = new Basket(); //created a basket
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);


            var controller = new BasketController(basketService, orderService);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;

            Assert.AreEqual(3, basketSummary.BasketCount); //checked that your basket count was 3
            Assert.AreEqual(25.00m, basketSummary.BasketTotal); // checked that your baskt total was 25.00
        }

        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            IRepository<Product> products = new MockContext<Product>(); // created a mock Product repository
            products.Insert(new Product() { Id = "1", Price = 10.00m }); // added products with an Id and Price so we can check the Id and calculations
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            IRepository<Basket> baskets = new MockContext<Basket>(); // mock Basket repo
            Basket basket = new Basket();
            // created two basket items
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1, BasketId = basket.Id });

            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);

            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders); // created an order service with our two items and 

            var controller = new BasketController(basketService, orderService); // passed it thru the controller
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket")
            {
                Value = basket.Id
            });

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            Order order = new Order(); 
            controller.Checkout(order); // expects order to be checked in

            //Assert
            Assert.AreEqual(2, order.OrderItems.Count); // makes sure we have two items in our order
            Assert.AreEqual(0, basket.BasketItems.Count); // makes sure basket items are empty

            Order orderInRepo = orders.Find(order.Id); // retrieve the order from the repo
            Assert.AreEqual(2, orderInRepo.OrderItems.Count); // makes sure we have two orders in our repo

        }
    }
}
