namespace MingleZone.Models
{
    public class PostTag
    {
        public Guid Id { get; set; }
        public int PostId { get; set; }
        public int TagId { get; set; }
        public Post Post { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
