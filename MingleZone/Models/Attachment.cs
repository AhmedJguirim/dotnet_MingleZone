using MingleZone.Utils;

namespace MingleZone.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        [SwaggerSchemaExample("don't use this :)")]
        public string FileName { get; set; } = null!;
        [SwaggerSchemaExample("tell the dev that he needs to remove the files controller")]
        public string FilePath { get; set; } = null!;

        public int PostId { get; set; }

        public virtual Post Post { get; set; } = null!;
    }
}
