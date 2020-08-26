using System;
using System.Web;
using System.Web.Mvc;

namespace OneFourThree.App_Code
{
    public class LoverFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (Convert.ToInt32(HttpContext.Current.Session["AccessLevel"]) != 2 || HttpContext.Current.Session["CurrentUserID"] == null)
            {
                filterContext.Result = new RedirectResult("~/PublicUser/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}