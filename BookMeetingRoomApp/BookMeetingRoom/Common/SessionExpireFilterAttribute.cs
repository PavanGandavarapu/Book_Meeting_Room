using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookMeetingRoom.Common
{
  public class SessionExpireFilterAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      HttpSessionStateBase session = filterContext.HttpContext.Session;
      Controller controller = filterContext.Controller as Controller;

      if (controller != null)
      {
        if (session != null && session["IsLoggedIn"] == null)
        {
          filterContext.Result =
                 new RedirectToRouteResult(
                     new RouteValueDictionary{{ "controller", "Account" },
                                          { "action", "Login" }

                                                   });
        }
      }

      base.OnActionExecuting(filterContext);
    }
  }
}