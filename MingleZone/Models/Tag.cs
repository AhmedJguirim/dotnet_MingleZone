using MingleZone.Utils;

namespace MingleZone.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [SwaggerSchemaExample("Sports")]
        public string Name { get; set; } = null!;
        public virtual ICollection<PostTag> Posts { get; set; }
        public DateTime CreatedDate { get; set; }
        public Tag()
        {
            CreatedDate = DateTime.Now;
            Posts = new HashSet<PostTag>();
        }
    }
}
