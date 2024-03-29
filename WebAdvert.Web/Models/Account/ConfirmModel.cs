﻿using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Account
{
    public class ConfirmModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name ="Email")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        public string? code { get; set; }
    }
}
