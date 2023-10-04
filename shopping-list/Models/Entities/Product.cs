using shopping_list.Models.Entities.Abstract;

namespace shopping_list.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<ProductShoppingList> ProductShoppingLists { get; set; }
    }
}
