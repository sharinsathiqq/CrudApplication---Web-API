using System.ComponentModel.DataAnnotations;

namespace CrudApplication.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // SuperAdmin, Admin, or Staff
    }
}
