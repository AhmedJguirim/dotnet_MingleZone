﻿namespace MingleZone.Models
{
    public class CommunityMembership
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int CommunityId { get; set; }
        public Community Community { get; set; }
    }
}
