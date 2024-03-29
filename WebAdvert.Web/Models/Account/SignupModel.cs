﻿using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Account
{
    public class SignupModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(8, ErrorMessage = "Password must be at least 6 characters long")]
        [Display(Name = "Passoword")]
        public string? Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and its confirmation does not match")]
        [Display(Name = "Confirm Passoword")]
        public string? ConfirmPassword { get; set; }

    }
}
