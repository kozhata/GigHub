using GigHub.Models;
using System;

namespace GigHub.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public NotificationType Type { get; set; }

        public DateTime? OriginalDateTime { get; set; }

        public string OriginalVenue { get; set; }

        public GigDto Gig { get; set; }
    }
}