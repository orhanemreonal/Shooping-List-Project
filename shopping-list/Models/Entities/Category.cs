using shopping_list.Models.Entities.Abstract;

namespace shopping_list.Models.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
