using System.ComponentModel.DataAnnotations;

namespace StudentService.Models
{
    public class Student
    {
        [Key]
        public string StudentId { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;   // Họ và tên đệm
        public string FirstName { get; set; } = string.Empty;  // Tên
        public string ClassName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}