using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
    {
        IStorageRepository<Product> productRepository;
        IStorageRepository<Basket> basketRepository;

        public const string BASKET_SESSION_NAME = "eCommerceBasket";

        public BasketService(IStorageRepository<Product> productRepository, 
            IStorageRepository<Basket> basketRepository)
        {
            this.basketRepository = basketRepository;
            this.productRepository = productRepository;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie httpCookie = httpContext.Request.Cookies.Get(BASKET_SESSION_NAME);
            Basket basket = new Basket();

            if (httpCookie != null)
            {
                string basketId = httpCookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketRepository.FindItem(basketId);
                }
                else
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
            basketRepository.InsertItem(basket);
            basketRepository.Commit();
            HttpCookie cookie = new HttpCookie(BASKET_SESSION_NAME);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddBasketItem(HttpContextBase httpContext, string productId)
        {

            Basket basket = GetBasket(httpContext, true);

            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            //Item is not in basket yet and is the first of its kind to be added in basket
            if (basketItem == null)
            {
                basketItem = new BasketItem();
                basketItem.BasketId = basket.Id;
                basketItem.ProductId = productId;
                basketItem.Quantity = 1;
            }
            else
            {
                basketItem.Quantity = basketItem.Quantity+1;
            }

            basketRepository.Commit();
        }

        public void RemoveBasketItem(HttpContextBase httpContext, string basketItemId)
        {

            Basket basket = GetBasket(httpContext, true);

            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == basketItemId);

            //Item is not in basket yet and is the first of its kind to be added in basket
            if (basketItem == null)
            {
                throw new Exception("Item not in Basket");
            }
            else
            {
                basket.BasketItems.Remove(basketItem);
            }

            basketRepository.Commit();
        }

        public void ReduceBasketItemQuantity(HttpContextBase httpContext, string basketItemId)
        {

            Basket basket = GetBasket(httpContext, true);

            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == basketItemId);

            //Item is not in basket yet and is the first of its kind to be added in basket
            if (basketItem == null)
            {
                throw new Exception("Item not in Basket");
            }
            else
            {
                basketItem.Quantity = basketItem.Quantity - 1;
            }

            basketRepository.Commit();
        }
    }
}
