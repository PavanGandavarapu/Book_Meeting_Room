using BookMeetingRoom.DataAccess.Repositories;
using BookMeetingRoom.DataAccess;
using BookMeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookMeetingRoom.Common;
using BookMeetingRoom.Enums;
using BookMeetingRoom.Models.ViewModels;
using System.IO;
using BookMeetingRoom.DataAccess.Entities;

namespace BookMeetingRoom.Controllers
{
  [SessionExpireFilter]
  public class AdminController : Controller
  {
    private readonly IMeetingTypeRepo _meetingTypeRepo;
    private readonly IRoomRepo _RoomRepo;
    private readonly IServiceCategoryRepo _categoryRepo;
    private readonly IClientRepo _clientRepo;

    public AdminController()
    {
      var context = new DbContext();
      _meetingTypeRepo = new MeetingTypeRepos(context); 
      _RoomRepo = new RoomRepos(context);
      _categoryRepo = new ServiceCategoryRepos(context);
      _clientRepo = new ClientRepos(context);
    }

    [Route("admin/meeting-type")]
    public ActionResult MeetingType()
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      var model = _meetingTypeRepo.GetAllMeetingTypes();
      ViewBag.Alert = TempData["Alert"];
      return View(model);
    }

    [Route("admin/meeting-type/add")]
    public ActionResult AddMeetingType()
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      return View(new MeetingTypeModel());
    }

    [Route("admin/meeting-type/add")]
    [HttpPost]    
    public ActionResult AddMeetingType(MeetingTypeModel model)
    {
      if (ModelState.IsValid)
      {
        if (_meetingTypeRepo.InsertMeetingType(model))
        {
          TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Meeting Type added successfully");
          return RedirectToAction(nameof(MeetingType));
        }
      }
      TempData["Alert"] = CommonServices.Alert(Alerts.Warning, "Error Inserting Meeting Type");
      return View(model);
    }

    [Route("admin/meeting-type/edit/{meetingTypeId}")]
    public ActionResult EditMeetingType(string meetingTypeId)
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      var model = _meetingTypeRepo.GetMeetingTypeById(meetingTypeId);
      if (model == null)
      {
        return RedirectToAction("NotFound", "Error");
      }
      return View(model);
    }

    [Route("admin/meeting-type/edit/{meetingTypeId}")]
    [HttpPost]
    public ActionResult EditMeetingType(string meetingTypeId,MeetingTypeModel model)
    {
      if (ModelState.IsValid)
      {
        if (_meetingTypeRepo.UpdateMeetingType(model))
        {
          TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Meeting Type updated successfully");
          return RedirectToAction(nameof(MeetingType));
        }
      }
      TempData["Alert"] = CommonServices.Alert(Alerts.Warning, "Error updating Meeting Type");
      return View(model);
    }

    [Route("admin/meeting-type/delete/{meetingTypeId}")]
    public ActionResult DeleteMeetingType(string meetingTypeId)
    {
      _meetingTypeRepo.DeleteMeetingType(meetingTypeId);
      TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Meeting Type deleted successfully");
      return RedirectToAction(nameof(MeetingType));
    }

    [Route("admin/room")]
    public ActionResult Room()
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      var roomListVm = new RoomListVM();
      List<RoomModel> models = _RoomRepo.GetAll();

      ViewBag.Alert = TempData["Alert"];

      return View(models);
    }

    [Route("admin/room/add")]
    public ActionResult AddRoom()
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      List<MeetingTypeModel> meetingTypeModel = _meetingTypeRepo.GetAllMeetingTypes();
      ViewBag.MeetingTypes = meetingTypeModel.Select(type => new SelectListItem
      {
        Text = type.Purpose,
        Value = type.Id
      });
      return View(new RoomModel());
    }

    [Route("admin/room/add")]
    [HttpPost]
    public ActionResult AddRoom(HttpPostedFileBase RoomImage, RoomModel roomModel)
    {
      string imageFileName = null;
      try
      {
        if (ModelState.IsValid)
        {
          imageFileName = UploadImage(RoomImage);
          roomModel.RoomImageFileName = imageFileName;
          if (_RoomRepo.Insert(roomModel))
          {
            TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Room inserted successfully");
            return RedirectToAction(nameof(Room));
          }
          RemoveImage(imageFileName);
        }
        //return View(roomModel);
      }
      catch
      {
        RemoveImage(imageFileName);
      }

      ViewBag.Alert = CommonServices.Alert(Alerts.Warning, "Error Inserting Room");
      List<MeetingTypeModel> meetingTypeModel = _meetingTypeRepo.GetAllMeetingTypes();
      ViewBag.MeetingTypes = meetingTypeModel.Select(type => new SelectListItem
      {
        Text = type.Purpose,
        Value = type.Id
      });
      return View(roomModel);
    }

    [Route("admin/room/edit/{roomId}")]
    public ActionResult EditRoom(string roomId)
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      if (roomId == null)
      {
        return RedirectToAction(nameof(Room));
      }

      try
      {
        List<MeetingTypeModel> meetingTypeModel = _meetingTypeRepo.GetAllMeetingTypes();
        ViewBag.MeetingTypes = meetingTypeModel.Select(type => new SelectListItem
        {
          Text = type.Purpose,
          Value = type.Id
        });

        var model = _RoomRepo.GetById(roomId);
        return View(model);
      }
      catch
      {
        TempData["Alert"] = CommonServices.Alert(Alerts.Warning, "Error Editing Room");
        return RedirectToAction(nameof(Room));
      }
    }


    [Route("admin/room/edit/{roomId}")]
    [HttpPost]
    public ActionResult EditRoom(string roomId, HttpPostedFileBase RoomImage, RoomModel roomModel)
    {
      string imageFileName = roomModel.RoomImageFileName;
      string oldImageFilename = null;
      try
      {
        // TODO: Add update logic here

        if (ModelState.IsValid)
        {
          if (RoomImage != null && RoomImage.ContentLength > 0)
          {
            oldImageFilename = roomModel.RoomImageFileName;
            imageFileName = UploadImage(RoomImage);
            roomModel.RoomImageFileName = imageFileName;
          }

          if (_RoomRepo.Update(roomModel))
          {
            TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Room updated successfully");
            RemoveImage(oldImageFilename);
            return RedirectToAction(nameof(Room));
          }
          else
          {
            //Remove uploaded image from folder
            if (RoomImage.ContentLength > 0)
            {
              RemoveImage(imageFileName);
            }
          }
        }

      }
      catch (Exception ex)
      {
        ViewBag.Alert = CommonServices.Alert(Alerts.Warning, ex.Message);
        if (RoomImage != null && RoomImage.ContentLength > 0)
        {
          RemoveImage(imageFileName);
        }
      }

      List<MeetingTypeModel> meetingTypeModel = _meetingTypeRepo.GetAllMeetingTypes();
      ViewBag.MeetingTypes = meetingTypeModel.Select(type => new SelectListItem
      {
        Text = type.Purpose,
        Value = type.Id
      });
      return View(roomModel);
    }

    [Route("admin/room/delete/{roomId}")]
    public ActionResult DeleteRoom(string roomId)
    {
      _RoomRepo.Delete(roomId);
      TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Room deleted successfully");
      return RedirectToAction(nameof(Room));
    }

    [Route("admin/category")]
    public ActionResult Category()
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      var model = _categoryRepo.GetAll();
      ViewBag.Alert = TempData["Alert"];
      return View(model);
    }

    [Route("admin/category/add")]
    public ActionResult AddCategory()
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      return View(new ServiceCategoryModel());
    }

    [Route("admin/category/add")]
    [HttpPost]
    public ActionResult AddCategory(ServiceCategoryModel model)
    {
      if (ModelState.IsValid)
      {
        if (_categoryRepo.Insert(model))
        {
          TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Category added successfully");
          return RedirectToAction(nameof(Category));
        }
      }
      ViewBag.Alert = CommonServices.Alert(Alerts.Warning, "Error Inserting category");
      return View(model);
    }

    [Route("admin/category/edit/{categoryId}")]
    public ActionResult EditCategory(string categoryId)
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      if (categoryId == null) return RedirectToAction(nameof(Category));

      var model = _categoryRepo.GetById(categoryId);
      if (model == null)
      {
        return RedirectToAction("NotFound", "Error");
      }
      return View(model);
    }

    [Route("admin/category/edit/{categoryId}")]
    [HttpPost]
    public ActionResult EditCategory(string categoryId, ServiceCategoryModel model)
    {
      if (ModelState.IsValid)
      {
        if (_categoryRepo.Update(model))
        {
          TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Category updated successfully");
          return RedirectToAction(nameof(Category));
        }
      }
      ViewBag.Alert = CommonServices.Alert(Alerts.Warning, "Error updating Category");
      return View(model);
    }

    [Route("admin/category/delete/{categoryId}")]
    public ActionResult DeleteCategory(string categoryId)
    {
      var category = _categoryRepo.GetById(categoryId);
      if (category == null)
      {
        TempData["ErrorMsg"] = "Sorry! the service category you are looking to delete is not found";
        return RedirectToAction("NotFound", "Error");
      }
      _categoryRepo.Delete(categoryId);
      TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Category deleted successfully");
      return RedirectToAction(nameof(Category));
    }

    [Route("admin/service-category-items/{categoryId}")]
    public ActionResult CategoryItems(string categoryId)
    {
      ViewBag.Alert = TempData["Alert"];
      var category = _categoryRepo.GetById(categoryId);
      if (category == null)
      {
        TempData["ErrorMsg"] = "Sorry! the service category you are looking is not found";
        return RedirectToAction("NotFound", "Error");
      }
      var serviceItemModels = _categoryRepo.GetAllItems(categoryId);
      ViewBag.CategoryId = category.Id;
      ViewBag.CategoryName = category.CategoryName;
      
      return View(serviceItemModels);
    }

    [Route("admin/service-category-items/add/{categoryId}")]
    public ActionResult AddItem(string categoryId)
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      var category = _categoryRepo.GetById(categoryId);
      if (category == null)
      {
        TempData["ErrorMsg"] = "Sorry! the service category you are looking is not found";
        return RedirectToAction("NotFound", "Error");
      }

      ServiceItemModel serviceItemModel = new ServiceItemModel();
      serviceItemModel.CategoryId = category.Id;
      serviceItemModel.ServiceCategoryModel = category;

      return View(serviceItemModel);
    }

    [HttpPost]
    [Route("admin/service-category-items/add/{categoryId}")]
    public ActionResult AddItem(string categoryId, ServiceItemModel model)
    {
      if (ModelState.IsValid)
      {
        if (_categoryRepo.InsertItem(model))
        {
          TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Item added successfully");
          return RedirectToAction(nameof(CategoryItems), new { categoryId = model.CategoryId });
        }
      }
      ViewBag.Alert = CommonServices.Alert(Alerts.Warning, "Error Inserting item");
      return View(model);
    }

    [Route("admin/service-category-items/edit/{categoryId}")]
    public ActionResult EditItem(string itemId)
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      ServiceItemModel serviceItemModel = _categoryRepo.GetByItemId(itemId);
      if (serviceItemModel == null)
      {
        TempData["ErrorMsg"] = "Sorry! the item you are looking is not found";
        return RedirectToAction("NotFound", "Error");
      }

      return View(serviceItemModel);
    }

    [HttpPost]
    [Route("admin/service-category-items/edit/{categoryId}")]
    public ActionResult EditItem(string itemId, ServiceItemModel model)
    {
      if (ModelState.IsValid)
      {
        if (_categoryRepo.UpdateItem(model))
        {
          TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Item updated successfully");
          return RedirectToAction(nameof(CategoryItems), new { categoryId = model.CategoryId });
        }
      }
      ViewBag.Alert = CommonServices.Alert(Alerts.Warning, "Error updating item");
      return View(model);
    }

    [Route("admin/service-category-items/delete/{categoryId}")]
    public ActionResult DeleteItem(string itemId)
    {
      var serviceItemModel = _categoryRepo.GetByItemId(itemId);
      if (serviceItemModel == null)
      {
        TempData["ErrorMsg"] = "Sorry! the item you are looking to delete is not found";
        return RedirectToAction("NotFound", "Error");
      }
      _categoryRepo.DeleteItem(itemId);
      TempData["Alert"] = CommonServices.Alert(Alerts.Success, "Item deleted successfully");
      return RedirectToAction(nameof(CategoryItems), new { categoryId = serviceItemModel.CategoryId });
    }

    //Clients
    [Route("admin/clients")]
    public ActionResult Clients()
    {
      if (!(bool)Session[CommonServices.IsAdmin])
      {
        return RedirectToAction("AccessDenied", "Error");
      }

      var clientModels = _clientRepo.GetAll();
      return View(clientModels);
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