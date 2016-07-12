using GigHub.Auth;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    [Authorize]
    [RoutePrefix("api/attendance")]
    public class AttendancesController : ApiController
    {
        private readonly CustomIdentityDbContext _dbContext;

        public AttendancesController()
        {
            _dbContext = new CustomIdentityDbContext();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Attend(AttandanceDto request)
        {
            string userId = User.Identity.GetUserId();

            if (_dbContext
                .Attendences
                .Any(x => x.GigId == request.GigId && x.UserId == userId))
            {
                return BadRequest("Already exist");
            }

            Attendance attendance = new Attendance
            {
                GigId = request.GigId,
                UserId = userId
            };

            _dbContext.Attendences.Add(attendance);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
