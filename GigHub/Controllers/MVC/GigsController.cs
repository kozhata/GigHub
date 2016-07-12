using GigHub.Auth;
using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers.MVC
{
    [RoutePrefix("gigs")]
    public class GigsController : Controller
    {
        private readonly CustomIdentityDbContext _dbContext;

        public GigsController()
        {
            _dbContext = new CustomIdentityDbContext();
        }

        [HttpGet]
        [Route("mine")]
        [Authorize]
        public ActionResult Mine()
        {
            string userId = User.Identity.GetUserId();
            var gigs = _dbContext.Gigs
                .Include(x => x.Genre)
                .Include(x => x.Artist)
                .Where(x => x.ArtistId == userId &&
                            DbFunctions.TruncateTime((DateTime?)x.Date) > DbFunctions.TruncateTime(DateTime.UtcNow) &&
                            !x.IsCanceled)
                .ToList();

            ViewBag.Title = "My Upcoming Gigs";

            return View("Mine", gigs);
        }

        [HttpGet]
        [Route("canceled")]
        [Authorize]
        public ActionResult Calceled()
        {
            string userId = User.Identity.GetUserId();
            var gigs = _dbContext.Gigs
                .Include(x => x.Genre)
                .Include(x => x.Artist)
                .Where(x => x.ArtistId == userId &&
                            DbFunctions.TruncateTime(x.Date) > DbFunctions.TruncateTime(DateTime.UtcNow) &&
                            x.IsCanceled)
                .ToList();
            ViewBag.Title = "My Cancelled Gigs";
            return View("Mine", gigs);
        }

        [Route("attending")]
        [Authorize]
        public ActionResult Attending()
        {
            string userId = User.Identity.GetUserId();

            List<Gig> gigs = _dbContext.Attendences
                .Where(x => x.UserId == userId)
                .Select(x => x.Gig)
                .Include(x => x.Artist)
                .Include(x => x.Genre)
                .ToList();

            GigsViewModel viewModel = new GigsViewModel
            {
                Gigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I am Attending",
                UserId = userId
            };

            return View("Gigs", viewModel);
        }

        [Route("create")]
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _dbContext.Genres.ToList(),
                Heading = "Add a gig"
            };
            return View("GigForm", viewModel);
        }

        [ValidateAntiForgeryToken]
        [Route("Create")]
        [HttpPost]
        [Authorize]
        public ActionResult Create(GigFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _dbContext.Genres.ToList();
                model.Heading = "Add a gig";
                return View("GigForm", model);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                GenreId = model.GenreId,
                CreatedOn = DateTime.UtcNow,
                Date = model.GetDate(),
                Venue = model.Venue
            };

            _dbContext.Gigs.Add(gig);
            _dbContext.SaveChanges();
            return RedirectToAction("Mine");
        }

        [Route("edit")]
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _dbContext.Gigs.FirstOrDefault(x => x.Id == id && x.ArtistId == userId);

            if (gig == null)
            {
                return HttpNotFound();
            }

            var viewModel = new GigFormViewModel
            {
                Genres = _dbContext.Genres.ToList(),
                Date = gig.Date.ToString("dd MMMM yyyy"),
                Time = gig.Date.ToString("HH mm"),
                GenreId = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit a gig",
                Id = gig.Id
            };

            return View("GigForm", viewModel);
        }


        [ValidateAntiForgeryToken]
        [Route("edit")]
        [HttpPost]
        [Authorize]
        public ActionResult Edit(GigFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _dbContext.Genres.ToList();
                model.Heading = "Edit a gig";
                return View("GigForm", model);
            }

            var userId = User.Identity.GetUserId();
            var gig = _dbContext.Gigs
                .Include(x => x.Attendances.Select(c => c.User))
                .FirstOrDefault(x => x.Id == model.Id && x.ArtistId == userId);

            gig.Modify(model.GetDate(), model.Venue, model.GenreId);

            _dbContext.SaveChanges();
            return RedirectToAction("Mine");
        }
    }
}