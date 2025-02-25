using System.ComponentModel.DataAnnotations;

namespace WebChat.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }

    }
}
