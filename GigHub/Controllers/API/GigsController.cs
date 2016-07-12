using GigHub.Auth;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    [Authorize]
    [RoutePrefix("api/gigs")]
    public class GigsController : ApiController
    {
        private readonly CustomIdentityDbContext _dbContext;

        public GigsController()
        {
            _dbContext = new CustomIdentityDbContext();
        }

        [HttpDelete]
        [Route("{id:int}/cancel")]
        public HttpResponseMessage Cancel(int id)
        {
            string userId = User.Identity.GetUserId();
            Gig gig = _dbContext.Gigs
                .Include(x => x.Attendances.Select(c => c.User))
                .SingleOrDefault(x => x.Id == id && x.ArtistId == userId);

            if (gig == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Gig with Id {id} not found.");
            }

            if (gig.IsCanceled)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Gig with Id {id} already canceled.");
            }

            gig.Cancel();

            _dbContext.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("{id:int}/uncancel")]
        public HttpResponseMessage Uncalncel(int id)
        {
            string userId = User.Identity.GetUserId();
            Gig gig = _dbContext.Gigs.SingleOrDefault(x => x.Id == id && x.ArtistId == userId);

            if (gig == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Gig with Id {id} not found.");
            }

            if (!gig.IsCanceled)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Gig with Id {id} isn't canceled.");
            }

            gig.UnCancel();

            _dbContext.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
