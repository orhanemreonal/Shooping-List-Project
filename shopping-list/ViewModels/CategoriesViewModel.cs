namespace shopping_list.ViewModels
{
    public class CategoriesViewModel
    {
        public List<CategoryViewModel> List { get; set; }
        public CategoryViewModel Category { get; set; }
        public CategoryViewModel CategoryToEdit { get; set; }
    }
}
