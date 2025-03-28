﻿using System.ComponentModel.DataAnnotations;

namespace WebChat.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
