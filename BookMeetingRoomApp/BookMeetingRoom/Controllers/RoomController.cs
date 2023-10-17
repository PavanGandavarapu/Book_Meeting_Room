using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess;
using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.DataAccess.Repositories;
using BookMeetingRoom.Enums;
using BookMeetingRoom.Models;
using BookMeetingRoom.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookMeetingRoom.Controllers
{
  [SessionExpireFilter]
  public class RoomController : Controller
  {
    private readonly IRoomRepo _RoomRepo;
    private readonly IMeetingTypeRepo _meetingTypeRepo;
    private readonly IServiceCategoryRepo _serviceRepo;
    public RoomController()
    {
      var context = new DbContext();
      _RoomRepo = new RoomRepos(context);
      _meetingTypeRepo = new MeetingTypeRepos(context);
      _serviceRepo = new ServiceCategoryRepos(context);
    }

    // GET: Room
    public ActionResult Index()
    {
      var roomListVm = new RoomListVM();
      roomListVm.Message = "Search and filter your desired meeting rooms";
      roomListVm.RoomModels = new List<RoomModel>();
      roomListVm.MeetingTypes = _meetingTypeRepo.GetAllMeetingTypes().Select(t => new SelectListItem
      {
        Text = t.Purpose,
        Value = t.Id
      });

      roomListVm.LocationList = _RoomRepo.GetAllRoomLocations().Select(loc => new SelectListItem
      {
        Text = loc,
        Value = loc
      });

      ViewBag.Alert = TempData["Alert"];

      return View(roomListVm);
    }

    [HttpPost]
    public ActionResult Index(RoomListVM roomListVM)
    {
      roomListVM.RoomModels = _RoomRepo.GetUnReservedRooms(roomListVM);
      roomListVM.MeetingTypes = _meetingTypeRepo.GetAllMeetingTypes().Select(t => new SelectListItem
      {
        Text = t.Purpose,
        Value = t.Id
      });

      roomListVM.LocationList = _RoomRepo.GetAllRoomLocations().Select(loc => new SelectListItem
      {
        Text = loc,
        Value = loc
      });

      Session["RoomListVM"] = roomListVM;

      return View(roomListVM);
    }

    // GET: Room/Details/5
    [Route("Room/Details/{roomId}")]
    public ActionResult Details(string roomId)
    {
      var roomModel = _RoomRepo.GetById(roomId);
      if (roomModel == null)
      {
        TempData["ErrorMsg"] = "The room you are lookin for is not found";
        return RedirectToAction("NotFound","Error");
      }

      var roomDetailsVM = new RoomDetailsVM();      
      roomDetailsVM.RoomModel = roomModel;
      roomDetailsVM.CategoryModels = _serviceRepo.GetAllCategoriesWithItems();

      //TempData["RoomListVM"] = (RoomListVM)TempData["RoomListVM"];
      //Session["RoomListVM"] = (RoomListVM)TempData["RoomListVM"];

      return View(roomDetailsVM);
    }

    [Route("Room/Details/{roomId}")]
    [HttpPost]
    public ActionResult Details(string roomId, RoomDetailsVM model)
    {
      model.RoomListVMs = (RoomListVM)Session["RoomListVM"];
      Session["RoomDetailVM"] = model; 
      return RedirectToAction("Index","Reservation");
    }

    public ActionResult RoomType()
    {
      return View();
    }

    private string UploadImage(HttpPostedFileBase RoomImage)
    {
      string uniqueFileName = null;
      if (RoomImage != null && RoomImage.ContentLength > 0)
      {
        string fileName = Path.GetFileName(RoomImage.FileName);
        uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
        string path = Path.Combine(Server.MapPath("~/Uploads/RoomImages"), uniqueFileName);
        RoomImage.SaveAs(path);
      }
      return uniqueFileName;
    }

    private void RemoveImage(string imageFileName)
    {
      if (imageFileName != null)
      {
        string path = Path.Combine(Server.MapPath("~/Uploads/RoomImages"), imageFileName);
        if (System.IO.File.Exists(path))
        {
          System.IO.File.Delete(path);
        }
      }
    }

  }
}
