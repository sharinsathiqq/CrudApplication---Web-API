namespace CrudApplication.DTOs
{
    public class StudentDTO
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string emailID { get; set; }
        public string phoneNumber { get; set; }
        public string dob { get; set; }  // String to control format
        public int departmentId { get; set; }
        public string? ProfilePhotoFile { get; set; }

        // New Address field
        public string? Address { get; set; } // Nullable, because address might not be mandatory
    }
}
