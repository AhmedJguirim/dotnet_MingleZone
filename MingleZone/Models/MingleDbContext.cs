using Microsoft.EntityFrameworkCore;

namespace MingleZone.Models
{
    public class MingleDbContext : DbContext
    {
        public MingleDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Community> Communities { get; set; } = null!;
        public DbSet<CommunityMembership> CommunityMemberships { get; set; } = null!;
        public DbSet<Attachment> Attachments { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
