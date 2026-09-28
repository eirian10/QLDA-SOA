using Microsoft.EntityFrameworkCore;

namespace RegistrationService.Models
{
    public class RegistrationDbContext : DbContext
    {
        public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : base(options) { }

        public DbSet<Registration> Registrations { get; set; }
    }
}