using System.Collections.Generic;
using System.Web;
using MyShop.Core.ViewModels;

namespace MyShop.Services
{
    public interface IBasketService
    {
        void AddBasketItem(HttpContextBase httpContext, string productId);
        List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext);
        BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext);
        void ReduceBasketItemQuantity(HttpContextBase httpContext, string basketItemId);
        void RemoveBasketItem(HttpContextBase httpContext, string basketItemId);
    }
}