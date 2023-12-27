namespace MingleZone.Models
{
    public class File
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;

        public int PostId { get; set; }

        public virtual Post Post { get; set; } = null!;
    }
}
