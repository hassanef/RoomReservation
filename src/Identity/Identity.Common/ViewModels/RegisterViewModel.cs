using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Services.Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام کاربری")]
        [Remote("ValidateUsername", "Register",
            AdditionalFields = nameof(Email), HttpMethod = "POST")]
        [RegularExpression("^[a-zA-Z_]*$", ErrorMessage = "لطفا تنها از حروف انگلیسی استفاده نمائید")]
        public string Username { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "(*)")]
        [StringLength(450)]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "(*)")]
        [StringLength(450)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Remote("ValidateUsername", "Register",
            AdditionalFields = nameof(Username), HttpMethod = "POST")]
        [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "(*)")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 6)]
        [Remote("ValidatePassword", "Register",
            AdditionalFields = nameof(Username), HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه‌ی عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "(*)")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه‌ی عبور")]
        [Compare(nameof(Password), ErrorMessage = "کلمات عبور وارد شده با هم تطابق ندارند")]
        public string ConfirmPassword { get; set; }
    }
}