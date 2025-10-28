using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SesliKitapWeb.Models
{
    public enum FollowStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }

    public class UserFollow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FollowerId { get; set; }

        [ForeignKey("FollowerId")]
        public virtual ApplicationUser Follower { get; set; }

        [Required]
        public string FollowingId { get; set; }

        [ForeignKey("FollowingId")]
        public virtual ApplicationUser Following { get; set; }

        public FollowStatus Status { get; set; } = FollowStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

