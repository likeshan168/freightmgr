
namespace Allocation.Allot.Pages
{
    using Serenity;
    using Serenity.Web;
    using System.Web.Mvc;

    [RoutePrefix("Allot/Api"), Route("{action=index}")]
    [PageAuthorize(typeof(Entities.ApiRow))]
    public class ApiController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Modules/Allot/Api/ApiIndex.cshtml");
        }
    }
}