using GigHub.Auth;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    [Authorize]
    [RoutePrefix("api/notifications")]
    public class NotificationsController : ApiController
    {
        private readonly CustomIdentityDbContext _dbContext;

        public NotificationsController()
        {
            _dbContext = new CustomIdentityDbContext();
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            string userId = User.Identity.GetUserId();

            List<Notification> notifications = _dbContext.UserNotifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .Select(x => x.Notification)
                .Include(x => x.Gig.Artist)
                .Include(x => x.Gig.Genre)
                .ToList();

            return notifications.Select(x => new NotificationDto
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                OriginalDateTime = x.OriginalDateTime,
                OriginalVenue = x.OriginalVenue,
                Type = x.Type,
                Gig = new GigDto
                {
                    Id = x.Gig.Id,
                    Date = x.Gig.Date,
                    Venue = x.Gig.Venue,
                    IsCanceled = x.Gig.IsCanceled,
                    Genre = new GenreDto
                    {
                        Id = x.Gig.Genre.Id,
                        Name = x.Gig.Genre.Name
                    },
                    Artist = new UserDto
                    {
                        Id = x.Gig.Artist.Id,
                        Name = x.Gig.Artist.Name
                    }
                }
            });
        }
    }
}
