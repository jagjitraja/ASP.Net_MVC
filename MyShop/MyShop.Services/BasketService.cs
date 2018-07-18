using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
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

            System.Diagnostics.Debug.WriteLine((basketItem==null)+"[][][][][][][][][][][][][][][][][][[]");
            //Item is not in basket yet and is the first of its kind to be added in basket
            if (basketItem == null)
            {
                basketItem = new BasketItem();
                basketItem.BasketId = basket.Id;
                basketItem.ProductId = productId;
                basketItem.Quantity = 1;


                System.Diagnostics.Debug.WriteLine((basketItem.Id)+"\n\n\n"+(basketItem.BasketId==basket.Id) + "****/*/*/*/*//*/*/*/**/*/*/*/*/*/*/*/*");

                basket.BasketItems.Add(basketItem);
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


            foreach(var item in basket.BasketItems)
            {
                System.Diagnostics.Debug.WriteLine(item.Id+" \n\n\n "+ item.ProductId+" \n\n\n "+basketItemId);
            }
            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.Id == basketItemId);

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

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                var result = (from b in basket.BasketItems
                              join p in productRepository.GetItems() on b.ProductId equals p.Id

                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  Name = p.Name,
                                  ImageURL = p.Image,
                                  Price = p.Price
                              }).ToList();
                return result;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
            
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext,false);

            BasketSummaryViewModel basketSummaryViewModel = new BasketSummaryViewModel(0,0);
            if (basket != null)
            {
                int? totalItemCount = (from i in basket.BasketItems
                                       select i.Quantity).Sum();

                //allows it to store null
                decimal? basketTotalValue = (from i in basket.BasketItems
                                             join p in productRepository.GetItems() on
                                             i.ProductId equals p.Id
                                             select i.Quantity * p.Price).Sum();


                //check if null, if null then assign it to 0
               
                basketSummaryViewModel.TotalBasketValue = basketTotalValue ?? 0;
                basketSummaryViewModel.TotalItemCount = totalItemCount ?? 0;
                
            }

            return basketSummaryViewModel;

        }


    }
}
