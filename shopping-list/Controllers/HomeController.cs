using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shopping_list.Data;
using shopping_list.Models.Entities;
using shopping_list.Models.Identity;
using shopping_list.Models.Role;
using shopping_list.ViewModels;
using System.Diagnostics;

namespace shopping_list.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MyContext _context;

        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, MyContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            //CheckRoles();
        }

        public IActionResult ShoppingList(int id)
        {
            //Hangi shoppingList ile işlem yapacağım?
            var shoppingList = _context.ShoppingList.ToList().Find(a => a.Id == id);
            
            //tüm ürünler
            var products = _context.Product.Select(a => new ProductViewModel
            {
                Id = a.Id,
                CategoryId = a.CategoryId,
                Name = a.Name
            }).ToList();

            //Listenin ürünleri
            var listProducts = _context.ProductShoppingList.Include(a => a.Product).Where(a => a.ShoppingListId == id).Select(a => new ProductViewModel
            {
                Id = a.Product.Id,
                CategoryId = a.Product.CategoryId,  
                Name = a.Product.Name,
                Description = a.Description
            }).ToList();

            foreach(var product in listProducts)
            {
                var category = _context.Category.Select(a => new CategoryViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToList().Find(a => a.Id == product.CategoryId);
                product.Category = category;
            }

            var model = new ShoppingListViewModel
            {
                List = listProducts,
                Products = products,
                Id = shoppingList.Id,
                Name =shoppingList.Name
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ShoppingList(ShoppingListViewModel model)
        {
            if(model.Product.Id != 0)
            {
                _context.ProductShoppingList.Add(new ProductShoppingList
                {
                    ProductId = model.Product.Id,
                    ShoppingListId = model.Id,
                    Description = model.Product.Description
                });
                var result = _context.SaveChanges();
            }
            return RedirectToAction(nameof(ShoppingList));
        }

        [HttpGet]
        public IActionResult ShoppingLists(int? id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            ShoppingListsViewModel model = new ShoppingListsViewModel();

            var shoppingLists = _context.ShoppingList.Where(a => a.CreatedUser == user.UserName).Select(x => new ShoppingListViewModel
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            foreach(var shoppingList in shoppingLists)
            {
                var products = _context.ProductShoppingList.Where(a => a.ShoppingListId == shoppingList.Id).Include(a => a.Product.Category).Select(a => new ProductViewModel
                {
                    Id = a.ProductId,
                    CategoryId = a.Product.CategoryId,
                    Name = a.Product.Name,
                    Category = new CategoryViewModel
                    {
                        Id = a.Product.Category.Id,
                        Name = a.Product.Category.Name,
                    }
                }).ToList();
                shoppingList.Products = products;
            }
            if (shoppingLists == null)
            {
                ViewBag.Mesaj = "Listelere Ulaşılamadı";
                return View();
            }

            
            if(id != 0)
            {
                var shoppingList = _context.ShoppingList.Select(a => new ShoppingListViewModel
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList().Find(a => a.Id == id);
                model.ShoppingListToEdit = shoppingList;
            }


            model.List = shoppingLists;
            return View(model);
        }

        [HttpPost]
        public IActionResult ShoppingLists(ShoppingListsViewModel model)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (model.ShoppingListToEdit != null)
            {
                var existingShoppingList = _context.ShoppingList.ToList().Find(a => a.Id == model.ShoppingListToEdit.Id);
                existingShoppingList.Name = model.ShoppingListToEdit.Name;
                _context.ShoppingList.Update(existingShoppingList);
                var result = _context.SaveChanges();
            }
            else
            {

                ShoppingList shoppingList = new ShoppingList()
                {

                    Name = model.ShoppingList.Name,
                    CreatedUser = user.UserName
                };
                _context.ShoppingList.Add(shoppingList);
                int result = _context.SaveChanges();

                if (result == 0)
                {
                    ViewBag.Mesaj = "Oluşturma Başarısız";
                }
            }
            

            return RedirectToAction(nameof(ShoppingLists), new { id = 0 });

        }

        [Authorize]
        public IActionResult DeleteShoppingList(int id)
        {
            var shoppingList = _context.ShoppingList.ToList().Find(a => a.Id == id);
            _context.ShoppingList.Remove(shoppingList);
            _context.SaveChanges();
            return RedirectToAction(nameof(ShoppingLists));
        }

        [HttpPost]
        public IActionResult DeleteProductListItem(ShoppingListViewModel model)
        {
            //gelen id productId
            //shopping list id?
            
            var row = _context.ProductShoppingList.ToList().Find(a => a.ProductId == model.ProductToDelete.ProductId && a.ShoppingListId == model.ProductToDelete.ShoppingListId);
            _context.ProductShoppingList.Remove(row);
            _context.SaveChanges();
            return RedirectToAction(nameof(ShoppingList), new { id = model.ProductToDelete.ShoppingListId });
        }

        public IActionResult Shopping(int id)
        {
            var listProducts = _context.ProductShoppingList.Include(a => a.Product).Where(a => a.ShoppingListId == id).Select(a => new ProductViewModel
            {
                Id = a.Product.Id,
                CategoryId = a.Product.CategoryId,
                Name = a.Product.Name,
                Description = a.Description
            }).ToList();

            foreach (var product in listProducts)
            {
                var category = _context.Category.Select(a => new CategoryViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToList().Find(a => a.Id == product.CategoryId);
                product.Category = category;
            }

            var model = new ShoppingViewModel
            {
                Products = listProducts,
                ShoppingListId = id
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Shopping(ShoppingViewModel model)
        {
            //isSelected alanı true olan kayıtlar silinecek
            var productShoppingList = _context.ProductShoppingList.ToList();
            if (model.Products != null)
            {
                foreach(var product in model.Products)
                {
                    if(product.isSelected)
                    {
                        var existingProduct = productShoppingList.Find(a => a.ProductId == product.Id && a.ShoppingListId == model.ShoppingListId);
                        _context.ProductShoppingList.Remove(existingProduct);
                    }
                }
                var result = _context.SaveChanges();
            }
             return RedirectToAction(nameof(ShoppingList), new { id = model.ShoppingListId });
        }
    }
}