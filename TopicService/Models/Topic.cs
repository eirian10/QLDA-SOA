using System.ComponentModel.DataAnnotations;

namespace TopicService.Models
{
    public class Topic
    {
        [Key]
        public string TopicId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Supervisor { get; set; } = string.Empty;
        public string Status { get; set; } = "Available"; // "Available" hoặc "Registered"
    }
}