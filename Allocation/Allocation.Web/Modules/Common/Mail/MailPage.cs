
namespace Allocation.Common.Pages
{
    using Serenity;
    using Serenity.Web;
    using System.Web.Mvc;

    [RoutePrefix("Common/Mail"), Route("{action=index}")]
    [PageAuthorize(typeof(Entities.MailRow))]
    public class MailController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Modules/Common/Mail/MailIndex.cshtml");
        }
    }
}