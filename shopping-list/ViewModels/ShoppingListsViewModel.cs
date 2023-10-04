namespace shopping_list.ViewModels
{
    public class ShoppingListsViewModel
    {
        public List<ShoppingListViewModel> List {  get; set; }
        public ShoppingListViewModel ShoppingList { get; set; }
        public ShoppingListViewModel ShoppingListToEdit { get; set; }
    }
}
