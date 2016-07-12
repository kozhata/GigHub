using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Notification
    {
        private Notification(NotificationType type, Gig gig)
        {
            if (gig == null)
            {
                throw new ArgumentNullException();
            }

            Type = type;
            Gig = gig;
            CreatedOn = DateTime.UtcNow;
        }

        protected Notification()
        {
            UserNotifications = new Collection<UserNotification>();
        }

        public int Id { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public NotificationType Type { get; private set; }

        public DateTime? OriginalDateTime { get; private set; }

        public string OriginalVenue { get; private set; }

        [Required]
        public int GigId { get; private set; }



        public Gig Gig { get; private set; }

        public ICollection<UserNotification> UserNotifications { get; set; }



        public static Notification GigCreated(Gig gig)
        {
            return new Notification(NotificationType.GigCreated, gig);
        }

        public static Notification GigUpdated(Gig gig, DateTime originalDateTime, string originalVenue)
        {
            return new Notification(NotificationType.GigUpdated, gig)
            {
                OriginalDateTime = originalDateTime,
                OriginalVenue = originalVenue
            };
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }

        public static Notification GigUncanceled(Gig gig)
        {
            return new Notification(NotificationType.GigUncanceled, gig);
        }
    }
}