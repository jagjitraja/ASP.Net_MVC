using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Basket : BaseObjectEntity
    {

        //Virtual enable lazy loading to load all items inside the basket when basket is requested
        public virtual ICollection<BasketItem> BasketItems { get; set; }


        public Basket()
        {
            this.BasketItems = new List<BasketItem>();
           
        }
    }
}
