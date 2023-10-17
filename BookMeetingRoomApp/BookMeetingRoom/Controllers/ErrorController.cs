using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BookMeetingRoom.Controllers
{
  public class ErrorController : Controller
  {

    public ActionResult Index()
    {
      ViewBag.ErrorMsg = TempData["ErrorMsg"];
      Response.StatusCode = 404;
      return View();
    }
    // GET: Error
    public ActionResult NotFound()
    {
      ViewBag.ErrorMsg = TempData["ErrorMsg"];
      Response.StatusCode = 404;
      return View();
    }

    public ActionResult AccessDenied()
    {
      Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      return View();
    }
  }
}