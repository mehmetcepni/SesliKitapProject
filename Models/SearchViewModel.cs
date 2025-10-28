using System.Collections.Generic;

namespace SesliKitapWeb.Models
{
    public class SearchViewModel
    {
        public string Query { get; set; } = string.Empty;
        public List<UserSearchResult> Users { get; set; } = new List<UserSearchResult>();
        public bool HasResults => Users.Count > 0;
    }

    public class UserSearchResult
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int BooksCount { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}

