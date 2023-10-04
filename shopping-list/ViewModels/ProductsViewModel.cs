using Microsoft.AspNetCore.Mvc.Rendering;
using shopping_list.Models.Entities;

namespace shopping_list.ViewModels
{
    public class ProductsViewModel
    {
        public List<ProductViewModel> List { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductViewModel ProductToEdit { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
