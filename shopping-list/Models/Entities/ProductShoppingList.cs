namespace shopping_list.Models.Entities
{
    public class ProductShoppingList
    {
        public int ProductId { get; set; }
        public int ShoppingListId { get; set; }
        public string Description { get; set; }
        public Product Product { get; set; }
        public ShoppingList ShoppingList { get; set; }
    }
}
