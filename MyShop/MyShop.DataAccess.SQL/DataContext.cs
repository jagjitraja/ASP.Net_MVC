using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        

        //Cause the DbContext class to connect with the Database declared in the Web.Config file
        public DataContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<Product> DBProducts { get; set; }
        public DbSet<ProductCategory> DBProductCategory { get; set; }
        public DbSet<Basket> DBBasket { get; set; }
        public DbSet<BasketItem> DBBasketItems { get; set; }
    }
}
