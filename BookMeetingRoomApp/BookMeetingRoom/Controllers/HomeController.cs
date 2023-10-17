using BookMeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookMeetingRoom.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Alert = TempData["Alert"];
      return View(new RegisterModel());
    }

    [HttpPost]
    public ActionResult Index(RegisterModel model)
    {
      if (ModelState.IsValid)
      {        
        TempData["RegisterModel"] = model;

        return RedirectToAction("ResigterLogin", "Account");
      }
      return View(new RegisterModel());
    }

  }
}