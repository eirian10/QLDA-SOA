namespace RegistrationService.Models
{
    public class RegistrationDetailDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string TopicId { get; set; } = string.Empty;
        public string TopicTitle { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; }
    }
}