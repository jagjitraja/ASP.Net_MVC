using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.DataAccess.InMemory;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository productRepository;
        ProductCategoryRepository productCategoryRepository;

        public ProductManagerController()
        {
            productRepository = new ProductRepository();
            productCategoryRepository = new ProductCategoryRepository();
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = productRepository.ProductListQuery().ToList(); 

            return View(products);
        }

        public ActionResult AddNewProduct()
        {

            ProductViewModel productViewModel = new ProductViewModel();

            productViewModel.Product = new Product();
            productViewModel.ProductCategories = productCategoryRepository.CategoryListQuery();
            return View(productViewModel);

        }
        [HttpPost]
        public ActionResult AddNewProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                productRepository.InsertProduct(product);
                productRepository.Commit();
                return RedirectToAction("Index");
            }

        }

        public ActionResult EditProduct(string ID)
        {
            Product prodToEdit = productRepository.Find(ID);

            if (prodToEdit != null)
            {

                ProductViewModel productViewModel = new ProductViewModel();
                productViewModel.Product = prodToEdit;
                productViewModel.ProductCategories = productCategoryRepository.CategoryListQuery();

                return View(productViewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult EditProduct(Product newProduct, string ID)
        {
            Product prodToEdit = productRepository.Find(ID);

            if (prodToEdit != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(newProduct);
                }
                else
                {
                    prodToEdit.Category = newProduct.Category;
                    prodToEdit.Description = newProduct.Description;
                    prodToEdit.Image = newProduct.Image;
                    prodToEdit.Name = newProduct.Name;
                    prodToEdit.Price = newProduct.Price;

                    productRepository.Commit();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult DeleteProduct(string id)
        {
            Product prodToDelete = productRepository.Find(id);

            if (prodToDelete != null)
            {
                return View(prodToDelete);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ActionName("DeleteProduct")]
        public ActionResult ConfirmDeleteProduct(string id)
        {
            Product prodToDelete = productRepository.Find(id);

            if (prodToDelete != null)
            {
                productRepository.DeleteProduct(prodToDelete);
                productRepository.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}