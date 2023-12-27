using Microsoft.Extensions.Hosting;

namespace MingleZone.Models
{
    public class CommunityMembership
    {
        public int Id { get; set; }
        public int CommunityId { get; set; }
        public int UserId { get; set; }
        public virtual Community Community { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public CommunityMembership()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}
