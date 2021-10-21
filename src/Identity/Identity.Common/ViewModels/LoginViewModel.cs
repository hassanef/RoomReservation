using System.ComponentModel.DataAnnotations;

namespace Identity.Common.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "(*)")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "RememberMe?")]
        public bool RememberMe { get; set; }
    }
}