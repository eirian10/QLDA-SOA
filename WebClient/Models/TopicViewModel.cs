namespace WebClient.Models
{
    public class TopicViewModel
    {
        public string TopicId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Supervisor { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
    }
}