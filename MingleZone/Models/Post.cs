using Microsoft.Extensions.Hosting;
using System.Net.Mail;

namespace MingleZone.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int UserId { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<File> Attachments { get; set; }
        public Post()
        {
            Tags = new HashSet<Tag>();
            Attachments = new HashSet<File>();
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}
