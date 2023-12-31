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
        public DbSet<Attachment> Attachments { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<CommunityMembership> CommunityMemberships { get; set; } = null!;
        public DbSet<MembershipRequest> MembershipRequests { get; set; } = null!;
        public DbSet<PostTag> PostTags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Community>()
                .HasOne(c => c.Admin)
                .WithMany(u => u.AdminOf)
                .HasForeignKey(c => c.AdminId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Community>()
                .HasOne(c => c.Admin)
                .WithMany(u => u.AdminOf)
                .HasForeignKey(c => c.AdminId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
