namespace MingleZone.Models
{
    public class MembershipRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int CommunityId { get; set; }
        public Community Community { get; set; } = null!;
        public DateTime? SentDate { get; set; }

        public MembershipRequest()
        {
            SentDate = DateTime.Now; 
        }

    }
}
