using GigHub.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class Attendance
    {
        public Gig Gig { get; set; }

        [Key]
        [Column(Order = 1)]
        public int GigId { get; set; }

        public User User { get; set; }

        [Key]
        [Column(Order = 2)]
        public string UserId { get; set; }
    }
}