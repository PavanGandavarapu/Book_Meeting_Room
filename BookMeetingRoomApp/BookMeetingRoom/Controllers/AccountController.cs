using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess;
using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.DataAccess.Repositories;
using BookMeetingRoom.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;
using CS = BookMeetingRoom.Common.CommonServices;

namespace BookMeetingRoom.Controllers
{
  public class AccountController : Controller
  {
    private readonly IClientRepo _clientRepo;
    private readonly IUserRepo _usertRepo;
    public AccountController()
    {
      var context = new DbContext();
      _clientRepo = new ClientRepos(context);
      _usertRepo = new UserRepos(context);
    }

    // GET: Account
    public ActionResult Login()
    {
      return View(new LoginModel());
    }

    [HttpPost]
    public ActionResult Login(LoginModel loginModel, string ReturnUrl)
    {
      var userModel = _usertRepo.GetUserByUsernamePassword(loginModel);
      if (userModel != null)
      {
        FormsAuthentication.SetAuthCookie(userModel.UserName, false);
        Session[CS.IsLoggedIn] = true;
        Session[CS.UserId] = userModel.Id;
        Session[CS.IsAdmin] = false;
        Session[CS.UserName] = _clientRepo.GetById(userModel.ClientId).FullName;
        Session[CS.ClientId] = userModel.ClientId;

        if (ReturnUrl != null)
        {
          return Redirect(ReturnUrl);
        }

        return RedirectToAction("Index", "Room");
      }

      ViewBag.Message = "Invalid Login Credentials";
      return View(loginModel);
    }

    [Route("admin/admin-login")]
    public ActionResult AdminLogin()
    {
      return View(new LoginModel());
    }

    [HttpPost]
    [Route("admin/admin-login/")]
    public ActionResult AdminLogin(LoginModel loginModel)
    {
      if (loginModel.UserName == "admin" && loginModel.Password == "admin@123")
      {
        FormsAuthentication.SetAuthCookie("admin", false);
        Session[CS.IsLoggedIn] = true;
        Session[CS.UserName] = "Admin";
        Session[CS.IsAdmin] = true;

        return RedirectToAction("Index", "Booking");
      }

      ViewBag.Message = "Invalid Login Credentials";
      return View(loginModel);
    }

    public ActionResult ResigterLogin()
    {
      try
      {
        var model = (RegisterModel)TempData["RegisterModel"];
        var clientModel = new ClientModel()
        {
          FirstName = model.FirstName,
          LastName = model.LastName,
          Email = model.Email,
          Phone = model.Phone,
          Organization = model.Organization,
          Status = true
        };

        string clientId = null;
        if (_clientRepo.Insert(clientModel, out clientId))
        {
          var test = clientModel.ClientId;
          var userModel = new UserModel()
          {
            ClientId = clientId,
            UserName = model.Email,
            Password = model.Password,
            Role = "Client",
            Status = true
          };
          _usertRepo.Insert(userModel);

          Session[CS.IsLoggedIn] = true;
          Session[CS.IsAdmin] = false;
          Session[CS.UserName] = $"{model.FirstName} {model.LastName}";
          Session[CS.ClientId] = clientId;

          return RedirectToAction("Index", "Room");
        }
      }
      catch (Exception ex)
      {
        if (ex.HResult == -2146233088)
        {
          TempData["Alert"] = CommonServices.Alert(Enums.Alerts.Warning, "Email Id already registerd");
          return RedirectToAction("Index", "Home");
        }
        TempData["Alert"] = CommonServices.Alert(Enums.Alerts.Warning, ex.Message);
        return RedirectToAction("Index", "Home");
      }
      TempData["Alert"] = CommonServices.Alert(Enums.Alerts.Warning, "Sorry!, unable to register");
      return RedirectToAction("Index", "Home");
    }

    public ActionResult Logout()
    {
      FormsAuthentication.SignOut();
      Session.Abandon();
      return RedirectToAction(nameof(Login));
    }

    [Route("account/change-password")]
    public ActionResult ChangePassword()
    {
      ChangePasswordModel model = new ChangePasswordModel
      {
        UserId = Session[CS.UserId].ToString()
      };
      return View(model);
    }

    [Route("account/change-password")]
    [HttpPost]
    public ActionResult ChangePassword(ChangePasswordModel model)
    {
      if(ModelState.IsValid)
      {
        var userModel = _usertRepo.GetById(model.UserId);
        userModel.Password = model.Password;
        if(_usertRepo.Update(userModel))
        {
          model = new ChangePasswordModel();
          ViewBag.Alert = "Your password changed successfully";
        }
      }
      return View(model);
    }

    public JsonResult IsEmailExist(string email)
    {
      return Json(!_clientRepo.IsEmailRegistered(email), JsonRequestBehavior.AllowGet);
    }


  }
}