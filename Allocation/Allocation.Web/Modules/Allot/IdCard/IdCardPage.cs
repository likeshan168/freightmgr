
namespace Allocation.Allot.Pages
{
    using Serenity;
    using Serenity.Web;
    using System.Web.Mvc;

    [RoutePrefix("Allot/IdCard"), Route("{action=index}")]
    [PageAuthorize(typeof(Entities.IdCardRow))]
    public class IdCardController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Modules/Allot/IdCard/IdCardIndex.cshtml");
        }
    }
}