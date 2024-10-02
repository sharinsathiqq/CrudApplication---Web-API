using System.ComponentModel.DataAnnotations;

namespace CrudApplication.Models
{
    public class Login
    {

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; } // You should later hash this password, but for now, it's plain text.
    }
}

