using Microsoft.AspNetCore.Mvc;
using shopping_list.ViewModels;

namespace shopping_list.Components
{
    public class ShoppingListItemViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ShoppingListViewModel model) 
        { 
            return View(model);
        }

    }
}
