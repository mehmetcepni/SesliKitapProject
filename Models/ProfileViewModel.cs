using System.Collections.Generic;

namespace SesliKitapWeb.Models
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsCurrentUser { get; set; }
        public List<Book> UserBooks { get; set; } = new List<Book>();
    }
}

