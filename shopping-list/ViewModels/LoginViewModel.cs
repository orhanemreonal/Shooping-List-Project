using System.ComponentModel.DataAnnotations;

namespace shopping_list.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "Email alanı gereklidir.")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
