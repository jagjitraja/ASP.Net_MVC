using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        public void InsertCategory(ProductCategory productCategory)
        {
            productCategories.Add(productCategory);
        }

        public void DeleteProduct(ProductCategory product)
        {
            ProductCategory categoryToDelete = productCategories.Find(p => p.ID == product.ID);
            if (categoryToDelete != null)
            {
                productCategories.Remove(categoryToDelete);
            }
            else
            {
                throw new Exception("Product Not Found");
            }

        }

        public void UpdateProduct(ProductCategory category)
        {
            ProductCategory categoryToUpdate = productCategories.Find(p => p.ID == category.ID);
            if (categoryToUpdate != null)
            {
                categoryToUpdate = category;
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public ProductCategory Find(string iD)
        {
            ProductCategory foundCategory = productCategories.Find(p => p.ID == iD);

            return foundCategory;
        }

        public IQueryable<ProductCategory> CategoryListQuery()
        {
            return productCategories.AsQueryable();
        }

    }
}
