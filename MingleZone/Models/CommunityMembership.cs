namespace MingleZone.Models
{
    public class CommunityMembership
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int CommunityId { get; set; }
        public Community Community { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public CommunityMembership()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
