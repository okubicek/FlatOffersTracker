using Microsoft.AspNetCore.Mvc;

namespace FlatOffersTracker.Web.Controllers
{
	[Route("api/[controller]")]
	public class FlatOffersController : Controller
    {
		[HttpGet("[action]")]
		public ActionResult Get()
        {
            return View();
        }
    }
}