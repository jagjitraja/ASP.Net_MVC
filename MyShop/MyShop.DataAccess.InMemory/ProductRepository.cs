using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["PRODUCTS"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>;
            }
        }

        public void Commit()
        {
            cache["PRODUCTS"] = products;
        }

        public void InsertProduct(Product product)
        {
            products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            Product prodToDelete = products.Find(p => p.ID == product.ID);
            if (prodToDelete != null)
            {
                products.Remove(prodToDelete);
            }
            else
            {
                throw new Exception("Product Not Found");
            }

        }

        public void UpdateProduct(Product product)
        {
            Product prodToUpdate = products.Find(p => p.ID == product.ID);
            if (prodToUpdate != null)
            {
                prodToUpdate = product;
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public IQueryable<Product> ProductListQuery()
        {
            return products.AsQueryable();
        }

    }
}
