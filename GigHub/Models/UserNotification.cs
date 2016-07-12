using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class UserNotification
    {
        public UserNotification(User user, Notification notification)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            if (notification == null)
            {
                throw new ArgumentNullException();
            }

            User = user;
            Notification = notification;
        }

        protected UserNotification()
        {

        }

        [Key]
        [Column(Order = 1)]
        public string UserId { get; private set; }

        [Key]
        [Column(Order = 2)]
        public int NotificationId { get; private set; }

        public bool IsRead { get; set; }


        public User User { get; private set; }

        public Notification Notification { get; private set; }
    }
}