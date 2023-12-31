namespace MingleZone.Models.outputModels
{
    public class PostsOut
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int CommunityId { get; set; }
        public string CommunityName { get; set; } = null!;
    }
}
