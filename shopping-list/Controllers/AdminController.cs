using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shopping_list.Data;
using shopping_list.Models.Entities;
using shopping_list.Models.Identity;
using shopping_list.ViewModels;

namespace shopping_list.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MyContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, MyContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Products(int? id)
        {
            var categories = _context.Category.Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() }).ToList();

            var products = _context.Product.Include(e => e.Category).Select(a => new ProductViewModel
            {
                Id = a.Id,
                Name = a.Name,
                CategoryId = a.CategoryId,
                Category = new CategoryViewModel
                {
                    Id = a.Category.Id,
                    Name = a.Category.Name,
                }
            }).ToList();

            if (id != 0)
            {
                var existingProduct = _context.Product.Select(a => new ProductViewModel
                {
                    Id = a.Id,
                    CategoryId = a.CategoryId,
                    Name = a.Name
                }).ToList().Find(a => a.Id == id);

                var model2 = new ProductsViewModel
                {
                    Categories = categories,
                    List = products,
                    ProductToEdit = existingProduct
                };
                return View(model2);
            }

            var model = new ProductsViewModel
            {
                Categories = categories,
                List = products
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Products(ProductsViewModel model)
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (model.ProductToEdit != null)
            {
                //Edit işlemi yapılacak
                var productExists = _context.Product.ToList().Find(a => a.Id == model.ProductToEdit.Id);
                productExists.Name = model.ProductToEdit.Name;
                productExists.CategoryId = model.ProductToEdit.CategoryId;
                _context.Product.Update(productExists);
                var resultUpdate = _context.SaveChanges();
            }
            else
            {
                //ekleme işlemi
                var product = new Product
                {
                    Name = model.Product.Name,
                    CategoryId = model.Product.CategoryId,
                    CreatedUser = user.UserName
                };

                _context.Product.Add(product);
                var result = _context.SaveChanges();

                if (result == 0)
                {
                    ViewBag.Mesaj = "Oluşturma Başarısız";
                }

            }
            return RedirectToAction(nameof(Products), "Admin", new { id = 0 });
        }

        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Product.ToList().Find(a => a.Id == id);
            _context.Product.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Products), "Admin");
        }

        public IActionResult Categories(int? id)
        {

            var categories = _context.Category.Select(a => new CategoryViewModel
            {
                Id = a.Id,
                Name = a.Name,
            }).ToList();

            if (id != 0)
            {
                var category = categories.Find(a => a.Id == id);
                var model2 = new CategoriesViewModel
                {
                    List = categories,
                    CategoryToEdit = category
                };
                return View(model2);
            }


            var model = new CategoriesViewModel
            {
                List = categories
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Categories(CategoriesViewModel model)
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (model.CategoryToEdit != null)
            {
                var categoryExists = _context.Category.ToList().Find(a => a.Id == model.CategoryToEdit.Id);
                categoryExists.Name = model.CategoryToEdit.Name;
                _context.Category.Update(categoryExists);
                var resultUpdate = _context.SaveChanges();

            }
            else
            {
                var category = new Category
                {
                    Name = model.Category.Name,
                    CreatedUser = user.UserName
                };

                _context.Category.Add(category);
                var result = _context.SaveChanges();

                if (result == 0)
                {
                    ViewBag.Mesaj = "Oluşturma Başarısız";
                }
            }

            return RedirectToAction(nameof(Categories), "Admin", new { id = 0 });
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Category.ToList().Find(a => a.Id == id);
            _context.Category.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Categories), "Admin");
        }

    }
}
