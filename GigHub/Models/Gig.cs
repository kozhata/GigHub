using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {
        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        [Key]
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        public User Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public Genre Genre { get; set; }

        [Required]
        public int GenreId { get; set; }

        public ICollection<Attendance> Attendances { get; set; }

        public void Cancel()
        {
            IsCanceled = true;

            Notification notification = Notification.GigCanceled(this);

            foreach (User attendee in Attendances.Select(x => x.User))
            {
                attendee.Notify(notification);
            }
        }

        public void UnCancel()
        {
            IsCanceled = false;

            Notification notification = Notification.GigUncanceled(this);

            foreach (User attendee in Attendances.Select(x => x.User))
            {
                attendee.Notify(notification);
            }
        }

        public void Modify(DateTime dateTime, string venue, int genreId)
        {
            Notification notification = Notification.GigUpdated(this, Date, Venue);

            Venue = venue;
            Date = dateTime;
            GenreId = genreId;

            foreach (User user in Attendances.Select(x => x.User))
            {
                user.Notify(notification);
            }
        }
    }
}