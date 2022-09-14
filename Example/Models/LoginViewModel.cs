using System.ComponentModel.DataAnnotations;

namespace Respitory.Example.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }
        public string IncorrectInput { get; set; }
        public string ReturnUrl { get; set; }
    }
}
