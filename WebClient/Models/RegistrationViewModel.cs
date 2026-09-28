namespace WebClient.Models
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string TopicId { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}