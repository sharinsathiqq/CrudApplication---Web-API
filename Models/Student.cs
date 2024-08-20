using System.ComponentModel.DataAnnotations;

public class Student
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string name { get; set; }

    [Required]
    [EmailAddress]
    public string emailID { get; set; }

    [Required]
    [Phone]
    public string phoneNumber { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly dob { get; set; }

    [Required]
    public int departmentId { get; set; }

    public string? ProfilePhotoFile { get; set; }  // Nullable string
}
