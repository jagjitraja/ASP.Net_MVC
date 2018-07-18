using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.DataAccess.InMemory;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System.IO;

namespace MyShop.Core.Contracts
{
    public class ProductManagerController : Controller
    {
        IStorageRepository<Product> productRepository;
        IStorageRepository<ProductCategory> productCategoryRepository;

        public ProductManagerController(IStorageRepository<Product> productRepository, 
            IStorageRepository<ProductCategory> categoryRepository)
        {
            this.productRepository = productRepository;
            this.productCategoryRepository = categoryRepository;
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = productRepository.GetItems().ToList();
            System.Diagnostics.Debug.WriteLine("--------\n---\n-\n-\n-\n--n\n-\n-\n-\n-\n-\n-\n---------"+products.ToString());
            return View(products);
        }

        public ActionResult AddNewProduct()
        {

            ProductViewModel productViewModel = new ProductViewModel();

            productViewModel.Product = new Product();
            productViewModel.ProductCategories = productCategoryRepository.GetItems();
            return View(productViewModel);

        }
        [HttpPost]
        public ActionResult AddNewProduct(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    System.Diagnostics.Debug.WriteLine((file == null) + "   " + product.Image);
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[][][][]]][][][][][][][][][][][][][][[][");
                }


                System.Diagnostics.Debug.WriteLine(product.ToString());
                productRepository.InsertItem(product);
                productRepository.Commit();
                return RedirectToAction("Index");
            }

        }

        public ActionResult EditProduct(string ID)
        {
            Product prodToEdit = productRepository.FindItem(ID);

            if (prodToEdit != null)
            {

                ProductViewModel productViewModel = new ProductViewModel();
                productViewModel.Product = prodToEdit;
                productViewModel.ProductCategories = productCategoryRepository.GetItems();

                return View(productViewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult EditProduct(Product newProduct, string ID, HttpPostedFileBase file)
        {
            Product prodToEdit = productRepository.FindItem(ID);

            if (prodToEdit != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(newProduct);
                }
                else
                {
                    if (file != null)
                    {
                        prodToEdit.Image = newProduct.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//ProductImages//") + prodToEdit.Image);
                    }
                    
                    prodToEdit.Category = newProduct.Category;
                    prodToEdit.Description = newProduct.Description;
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
            Product prodToDelete = productRepository.FindItem(id);

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
            Product prodToDelete = productRepository.FindItem(id);

            if (prodToDelete != null)
            {
                productRepository.Delete(id);
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