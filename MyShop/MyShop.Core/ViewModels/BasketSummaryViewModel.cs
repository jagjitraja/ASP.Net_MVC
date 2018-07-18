using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class BasketSummaryViewModel
    {
        public string BasketId { get; set; }
        public int TotalItemCount { get; set; }
        public decimal TotalBasketValue { get; set; }


        public BasketSummaryViewModel() { }

        public BasketSummaryViewModel(int basketCount, decimal basketValue)
        {
            this.TotalItemCount = basketCount;
            this.TotalBasketValue = basketValue;
        }
    }
}
