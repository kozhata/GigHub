using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Auth;
using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.MVC
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        private readonly CustomIdentityDbContext _dbContext;

        public HomeController()
        {
            _dbContext = new CustomIdentityDbContext();
        }

        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            List<Gig> upcommingGigs = _dbContext.Gigs
                .Include(x => x.Artist)
                .Include(x => x.Genre)
                .Where(x =>
                        DbFunctions.TruncateTime((DateTime?) x.Date) > DbFunctions.TruncateTime(DateTime.UtcNow) &&
                        !x.IsCanceled)
                .ToList();

            GigsViewModel viewModel = new GigsViewModel
            {
                Gigs = upcommingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcomming Gigs",
                UserId = User.Identity.GetUserId()
            };

            return View("Gigs", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}