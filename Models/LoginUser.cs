
using System.ComponentModel.DataAnnotations;

namespace LoginRegistration.Models
{
    public class LoginUser
    {
        [EmailAddress]
        [Required]

        public string Email {get; set;}
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}