using shopping_list.Models.Entities;

namespace shopping_list.ViewModels
{
    public class ShoppingListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductViewModel Product { get; set; }
        public List<ProductViewModel> Products {  get; set; }
        public List<ProductViewModel> List { get; set; }
        public ProductShoppingListViewModel ProductToDelete { get; set; }
    }
}
