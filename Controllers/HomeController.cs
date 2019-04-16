using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [Route("products")]
        [HttpGet]
        public IActionResult Products()
        {
            List<Product> AllProducts = dbContext.Products.ToList();
            ViewBag.AllProducts = AllProducts;
            return View();
        }

        [Route("addcategorytoproduct")]
        [HttpPost]
        public IActionResult AddCategoryToProduct(Category categorytoaddtoproduct)
        {
            // System.Console.WriteLine(newProduct.Name);
            // System.Console.WriteLine(newProduct.Description);
            // System.Console.WriteLine(newProduct.Price);
            // dbContext.Add(newProduct);
            // dbContext.SaveChanges();
            // System.Console.WriteLine("this is the");
            // System.Console.WriteLine(categorytoaddtoproduct.Name);
            // System.Console.WriteLine("the product to attach to");
            // System.Console.WriteLine(HttpContext.Session.GetInt32("ProductViewed"));
            Association newAssociation = new Association();
            newAssociation.ProductId = (int)(HttpContext.Session.GetInt32("ProductViewed"));
            newAssociation.CategoryId = Int32.Parse(categorytoaddtoproduct.Name);
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            return RedirectToAction("ShowProduct", new {productid = HttpContext.Session.GetInt32("ProductViewed")});
        }

        [Route("addproducttocategory")]
        [HttpPost]
        public IActionResult AddProductToCategory(Product producttoaddtocategory)
        {

            // System.Console.WriteLine("this is the product to add to this category");
            // System.Console.WriteLine(producttoaddtocategory.Name);
            // System.Console.WriteLine("the category to attach to");
            // System.Console.WriteLine(HttpContext.Session.GetInt32("CategoryViewed"));

            Association newAssociation = new Association();
            newAssociation.CategoryId = (int)(HttpContext.Session.GetInt32("CategoryViewed"));
            newAssociation.ProductId = Int32.Parse(producttoaddtocategory.Name);
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            return RedirectToAction("ShowCategory", new {categoryid = HttpContext.Session.GetInt32("CategoryViewed")});
        }

        [Route("products")]
        [HttpPost]
        public IActionResult Products(Product newProduct)
        {
            // System.Console.WriteLine(newProduct.Name);
            // System.Console.WriteLine(newProduct.Description);
            // System.Console.WriteLine(newProduct.Price);
            dbContext.Add(newProduct);
            dbContext.SaveChanges();
            return RedirectToAction("Products");
        }

        [Route("categories")]
        [HttpGet]
        public IActionResult Categories()
        {
            List<Category> AllCategoriesList = dbContext.Categories.ToList();
            ViewBag.AllCategories= AllCategoriesList;
            return View();
        }

        [Route("categories")]
        [HttpPost]
        public IActionResult Categories(Category newCategory)
        {
            // System.Console.WriteLine(newCategory.Name);
            dbContext.Add(newCategory);
            dbContext.SaveChanges();
            return RedirectToAction("Categories");
        }

        [Route("products/{productid}")]
        [HttpGet]
        public IActionResult ShowProduct(int productid)
        {
            HttpContext.Session.SetInt32("ProductViewed",productid);
            var productWithCategories = dbContext.Products
                .Include(product => product.Categories)
                .ThenInclude(association => association.Category)
                .FirstOrDefault(product => product.ProductId == productid);
            List<int> takenCategories = new List<int>();
            foreach(var i in productWithCategories.Categories) 
            {
                takenCategories.Add(i.Category.CategoryId);
            }
            List<Category> untakenCategories = new List<Category>();
            foreach(var m in dbContext.Categories.ToList()) 
            {
                if (!takenCategories.Contains(m.CategoryId)) 
                {
                    untakenCategories.Add(m);
                }
            }
            ViewBag.AvailableCategories = untakenCategories;
            return View(productWithCategories);
        }

        [Route("category/{categoryid}")]
        [HttpGet]
        public IActionResult ShowCategory(int categoryid)
        {
            HttpContext.Session.SetInt32("CategoryViewed",categoryid);
            var CategoriesWithProducts = dbContext.Categories
                .Include(category => category.Products)
                .ThenInclude(association => association.Product)
                .FirstOrDefault(category => category.CategoryId == categoryid);

            List<int> takenProducts = new List<int>();
            foreach(var i in CategoriesWithProducts.Products) 
            {
                takenProducts.Add(i.Product.ProductId);
            }
            List<Product> untakenProducts = new List<Product>();
            foreach(var n in dbContext.Products.ToList()) 
            {
                if (!takenProducts.Contains(n.ProductId)) 
                {
                    untakenProducts.Add(n);
                }
            }
            ViewBag.AvailableProducts = untakenProducts;
            return View(CategoriesWithProducts);
        }

    }
}
