using GigHub.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GigHub.Auth
{
    public class CustomIdentityDbContext : IdentityDbContext<User>
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendences { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        public CustomIdentityDbContext()
            : base("GigHub", throwIfV1Schema: false)
        {
        }

        public static CustomIdentityDbContext Create()
        {
            return new CustomIdentityDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .HasRequired(x => x.Gig)
                .WithMany(x => x.Attendances)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Followers)
                .WithRequired(x => x.Followee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Followees)
                .WithRequired(x => x.Follower)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserNotification>()
                .HasRequired(x => x.User)
                .WithMany(x => x.UserNotifications)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}