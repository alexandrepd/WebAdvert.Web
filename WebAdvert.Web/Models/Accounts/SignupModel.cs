using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Accounts
{
    public class SignupModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Please type your email")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [Display(Name = "Passoword")]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and its confirmation does not match")]
        public string ConfirmPassword { get; set; }

    }
}
