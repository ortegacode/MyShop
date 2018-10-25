using MyShop.Core.Contracts; // added
using MyShop.Core.Models;  // added
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;  // added

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket"; // to identify a particular cookie we want using the eCommerce string

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)  // if the cookie exists
            {
                string basketId = cookie.Value; // if they got a cookie it will extract a value
                if (!string.IsNullOrEmpty(basketId)) // to ensure the value from the cookie is not null
                {
                    basket = basketContext.Find(basketId); // if not null it will load the basket from basketContext
                }

                else  // if basket id was null
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }

            }

            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;

        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket); // inserts into db
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName); // creates a cookie to the user machine
            cookie.Value = basket.Id; // added a value to the cookie
            cookie.Expires = DateTime.Now.AddDays(1); // sets the cookie experation date (1) Day
            httpContext.Response.Cookies.Add(cookie); //added cookie to send back to the user using httpContext.Response

            return basket; 
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true); // because we are inserting an item we need to make sure the basket is created so the create if null = true

            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);
            // see if theres already a basket item in the users basket with this product id

            if (item == null)  // does that product exist in the basket
            {
                item = new BasketItem() // if it doesnt we create a new item and we set the following fields
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item); // adds all items to the basket
            }

            else
            {
                item.Quantity = item.Quantity + 1;
            }

            basketContext.Commit(); // saves changes we made
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item= basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
              
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                var results = (from b in basket.BasketItems
                              join p in productContext.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price
                              }
                              ).ToList();

                return results;
            }

            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);

            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                model.BasketCount = basketCount ?? 0; // if there ia a basket count return the value if null return 0
                model.BasketTotal = basketTotal ?? decimal.Zero; // a well defined zero

                return model;


            }

            else
            {
                return model;
            }
        }
    }
}
