﻿using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryController : Controller
    {
        IStorageRepository<ProductCategory> productCategoryRepository;

        public ProductCategoryController(IStorageRepository<ProductCategory> categoryRepository)
        {
            this.productCategoryRepository = categoryRepository;
        }
        
        public ActionResult Index()
        {
            List<ProductCategory> categories = productCategoryRepository.GetItems().ToList();

            return View(categories);
        }

        public ActionResult AddNewCategory()
        {

            return View();

        }
        [HttpPost]
        public ActionResult AddNewCategory(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {
                productCategoryRepository.InsertItem(productCategory);
                productCategoryRepository.Commit();
                return RedirectToAction("Index");
            }

        }

        public ActionResult EditCategory(string ID)
        {
            ProductCategory categoryToEdit = productCategoryRepository.FindItem(ID);

            if (categoryToEdit != null)
            {
                return View(categoryToEdit);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult EditCategory(ProductCategory newProductCategory, string ID)
        {
            ProductCategory categoryToEdit = productCategoryRepository.FindItem(ID);

            if (categoryToEdit != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(newProductCategory);
                }
                else
                {
                    categoryToEdit.CategoryName = newProductCategory.CategoryName;

                    productCategoryRepository.Commit();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult DeleteCategory(string id)
        {
            ProductCategory categoryToDelete = productCategoryRepository.FindItem(id);

            if (categoryToDelete != null)
            {
                return View(categoryToDelete);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ActionName("DeleteCategory")]
        public ActionResult ConfirmDeleteCategory(string id)
        {
            ProductCategory categoryToDelete = productCategoryRepository.FindItem(id);

            if (categoryToDelete != null)
            {
                productCategoryRepository.Delete(categoryToDelete);
                productCategoryRepository.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}