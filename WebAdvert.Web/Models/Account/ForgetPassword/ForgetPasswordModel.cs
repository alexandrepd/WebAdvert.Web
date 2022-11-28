
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Account.ForgetPassword
{
    public class ForgetPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [DisplayName("Email")]
        public string? Email { get; set; }
    }
}
