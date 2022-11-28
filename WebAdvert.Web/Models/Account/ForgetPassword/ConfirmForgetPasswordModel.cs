
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Account.ForgetPassword
{
    public class ConfirmForgetPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        public string? Code { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(8, ErrorMessage = "Password must be at least 6 characters long")]
        [Display(Name = "Passoword")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and its confirmation does not match")]
        public string? ConfirmPassword { get; set; }


    }
}
