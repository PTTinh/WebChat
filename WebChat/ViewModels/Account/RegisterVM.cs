using System.ComponentModel.DataAnnotations;

namespace WebChat.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [MinLength(4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")] 
        public string ConfirmPassword { get; set; }

    }
}
