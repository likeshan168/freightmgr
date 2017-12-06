
namespace Allocation.Allot.Pages
{
    using Serenity;
    using Serenity.Web;
    using System.Web.Mvc;

    [RoutePrefix("Allot/DeclarationData"), Route("{action=index}")]
    [PageAuthorize(typeof(Entities.DeclarationDataRow))]
    public class DeclarationDataController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Modules/Allot/DeclarationData/DeclarationDataIndex.cshtml");
        }
    }
}