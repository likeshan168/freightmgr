
namespace Allocation.Administration.Pages
{
    using Serenity;
    using Serenity.Web;
    using System.Web.Mvc;

    [RoutePrefix("Administration/Tenants"), Route("{action=index}")]
    [PageAuthorize(typeof(Entities.TenantsRow))]
    public class TenantsController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Modules/Administration/Tenants/TenantsIndex.cshtml");
        }
    }
}