using MyShop.Core;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IStorageRepository<Product> productRepository;
        IStorageRepository<ProductCategory> categoryRepository;

        public HomeController(IStorageRepository<Product> products, IStorageRepository<ProductCategory> categories)
        {
            this.productRepository = products;
            this.categoryRepository = categories;
        }
        public ActionResult Index(string Category = null)
        {

            List<Product> products;
            List<ProductCategory> productCategories = categoryRepository.GetItems().ToList();

            if (Category != null)
            {
                products = productRepository.GetItems().Where(p => p.Category == Category).ToList();
            }
            else
            {
                products = productRepository.GetItems().ToList();
            }

            ProductListViewModel productListViewModel = new ProductListViewModel();
            productListViewModel.Products = products;
            productListViewModel.ProductCategories = productCategories;

            return View(productListViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ItemDetails(string Id)
        {
            Product product = productRepository.FindItem(Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }
    }
}