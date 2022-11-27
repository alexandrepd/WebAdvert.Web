
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Password is required")]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "~Remember me")]
        public bool RememberMe { get; set; }
    }
}
