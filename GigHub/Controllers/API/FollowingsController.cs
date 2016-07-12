using GigHub.Auth;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    [Authorize]
    [RoutePrefix("api/followings")]
    public class FollowingsController : ApiController
    {
        private readonly CustomIdentityDbContext _dbContext;

        public FollowingsController()
        {
            _dbContext = new CustomIdentityDbContext();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Follow(FollowingDto followDto)
        {
            var userId = User.Identity.GetUserId();
            if (userId.Equals(followDto.FolloweeId))
            {
                return BadRequest("Can not follow yourself.");
            }

            if (_dbContext.Followings
                .Any(x => x.FollowerId == userId && x.FolloweeId == followDto.FolloweeId))
            {
                return BadRequest("You already follow this user.");
            }

            var following = new Following
            {
                FolloweeId = followDto.FolloweeId,
                FollowerId = userId
            };

            _dbContext.Followings.Add(following);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
