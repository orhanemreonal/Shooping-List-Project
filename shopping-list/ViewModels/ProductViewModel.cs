namespace shopping_list.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool isSelected { get; set; } = false;
        public CategoryViewModel Category { get; set; }
    }
}
