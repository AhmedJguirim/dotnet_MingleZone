namespace MingleZone.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual ICollection<Post>  Posts { get; set; }
        public virtual ICollection<CommunityMembership> CommunityMemberships { get; set; }

        public User()
        {
            CreatedDate = DateTime.Now;
            Posts = new HashSet<Post>();
            CommunityMemberships = new HashSet<CommunityMembership>();
            UpdatedDate = DateTime.Now;
        }

    }
}







//public User(string name , string email,string password,DateTime birthdate)
//{
//    this.Name = name;
//    this.Email = email;
//    this.Password = password;
//    this.BirthDate = birthdate;
//    CreatedDate = DateTime.Now;
//    Posts = new HashSet<Post>();
//    UpdatedDate = DateTime.Now;
//}