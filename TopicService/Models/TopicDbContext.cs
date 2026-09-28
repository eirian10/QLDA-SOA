using Microsoft.EntityFrameworkCore;

namespace TopicService.Models
{
    public class TopicDbContext : DbContext
    {
        public TopicDbContext(DbContextOptions<TopicDbContext> options) : base(options) { }

        public DbSet<Topic> Topics { get; set; }
    }
}