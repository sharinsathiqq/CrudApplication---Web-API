using System.ComponentModel.DataAnnotations;

namespace CrudApplication.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
       

        [Required]
        [StringLength(100)]
       
        public string DepartmentName { get; set; }
    }
}
