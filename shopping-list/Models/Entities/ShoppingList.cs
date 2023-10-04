using shopping_list.Models.Entities.Abstract;

namespace shopping_list.Models.Entities
{
    public class ShoppingList : BaseEntity
    {
        public string Name { get; set; }
        public bool IsShoppingStarted { get; set; }
        public List<ProductShoppingList> ProductShoppingLists { get; set; }
    }
}
