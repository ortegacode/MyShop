using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.ViewModels;

namespace MyShop.Services
{
    public class OrderService :IOrderService 
    {
        IRepository<Order> orderContext; // repository of our orders
        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems) //created when we implemented IOrderService
        {
            foreach (var item in basketItems) // iterated thru our basket items
            {
                baseOrder.OrderItems.Add(new OrderItem() // for each item we will add it to the baseOrder
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });
            }

            orderContext.Insert(baseOrder); // insert items into baseOrder
            orderContext.Commit(); // saved it
        }
    }
}
