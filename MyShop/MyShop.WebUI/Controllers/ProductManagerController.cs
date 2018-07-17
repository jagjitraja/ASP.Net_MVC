using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.DataAccess.InMemory;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository productRepository;

        public ProductManagerController()
        {
            productRepository = new ProductRepository();
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = productRepository.ProductListQuery().ToList(); 

            return View(products);
        }

        public ActionResult AddNewProduct()
        {

            return View();

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
                return View(prodToEdit);
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