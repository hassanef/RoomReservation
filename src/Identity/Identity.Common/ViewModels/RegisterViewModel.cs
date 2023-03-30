using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Services.Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "(*)")]
        [Display(Name = "Username")]
        [Remote("ValidateUsername", "Register",
            AdditionalFields = nameof(Email), HttpMethod = "POST")]
        [RegularExpression("^[a-zA-Z_]*$", ErrorMessage = "Should be use English character")]
        public string Username { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "(*)")]
        [StringLength(450)]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        [Required(ErrorMessage = "(*)")]
        [StringLength(450)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Remote("ValidateUsername", "Register",
            AdditionalFields = nameof(Username), HttpMethod = "POST")]
        [EmailAddress(ErrorMessage = "Enter valid email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "(*)")]
        [StringLength(100, ErrorMessage = "{0} should be {2} and maximum {1} character.", MinimumLength = 6)]
        [Remote("ValidatePassword", "Register",
            AdditionalFields = nameof(Username), HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "(*)")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare(nameof(Password), ErrorMessage = "Password is not the same")]
        public string ConfirmPassword { get; set; }
    }
}