namespace MingleZone.Models
{
    public class Community
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<CommunityMembership> CommunityMemberships { get; set; }

        public Community()
        {
            CreatedDate = DateTime.Now;
            Posts = new HashSet<Post>();
            Users = new HashSet<User>();
            CommunityMemberships = new HashSet<CommunityMembership>();
            UpdatedDate = DateTime.Now;
        }
    }
}
