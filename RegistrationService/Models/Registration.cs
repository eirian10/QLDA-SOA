using System.ComponentModel.DataAnnotations;

namespace RegistrationService.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string TopicId { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }
}