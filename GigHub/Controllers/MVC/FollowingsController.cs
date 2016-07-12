using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Auth;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.MVC
{
    [Authorize]
    [RoutePrefix("followings")]
    public class FollowingsController : Controller
    {
        private readonly CustomIdentityDbContext _dbContext;

        public FollowingsController()
        {
            _dbContext = new CustomIdentityDbContext();
        }

        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            List<User> users = _dbContext.Followings
                .Where(x => x.FollowerId == userId)
                .Include(x => x.Followee)
                .Select(x => x.Followee)
                .ToList();

            return View(users);
        }
    }
}