using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess;
using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.DataAccess.Repositories;
using BookMeetingRoom.Enums;
using BookMeetingRoom.Models;
using BookMeetingRoom.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookMeetingRoom.Controllers
{
  [SessionExpireFilter]
  public class ReservationController : Controller
  {
    private readonly IBookingRepo _bookingRepo;

    public ReservationController()
    {
      var context = new DbContext();
      _bookingRepo = new BookingRepos(context);
    }

    // GET: Reservation
    public ActionResult Index()
    {
      Session["ReservationConfirmationVM"] = "";
      double totalAmount = 0;
      var roomDetailVM = (RoomDetailsVM)Session["RoomDetailVM"];

      var reservationCnfModel = new ReservationConfirmationViewModel();
      reservationCnfModel.RoomId = roomDetailVM.RoomModel.RoomId;
      reservationCnfModel.RoomTitle = roomDetailVM.RoomModel.Title;
      reservationCnfModel.Attendees = roomDetailVM.RoomListVMs.Capacity.Value;
      reservationCnfModel.ReservationDate = roomDetailVM.RoomListVMs.ReservationDate;
      reservationCnfModel.FromTime = roomDetailVM.RoomListVMs.FromTime;
      reservationCnfModel.ToTime = roomDetailVM.RoomListVMs.ToTime;      
      reservationCnfModel.RoomPricePerHour = roomDetailVM.RoomModel.Price.Value;

      TimeSpan hours = roomDetailVM.RoomListVMs.ToTime - roomDetailVM.RoomListVMs.FromTime;
      reservationCnfModel.TotalHours = $"{hours.Hours}Hr : {hours.Minutes}M";

      var roomTotalAMount = hours.TotalHours * roomDetailVM.RoomModel.Price.Value;
      reservationCnfModel.RoomTotalAmount = roomTotalAMount;

      totalAmount = roomTotalAMount;

      var serviceCategoryModels = new List<ServiceCategoryModel>();
      foreach(var category in roomDetailVM.CategoryModels)
      {
        foreach (var item in category.ItemModels)
        {          
          if (item.IsSelected)
          {
            var serviceCategoryModel = new ServiceCategoryModel();
            serviceCategoryModel.Id = category.Id;
            serviceCategoryModel.CategoryName = category.CategoryName;
            serviceCategoryModel.ItemModels = new List<ServiceItemModel>();
            var itemAmount = (item.Quantity.Value * item.Price.Value);
            item.Amount = itemAmount;
            serviceCategoryModel.ItemModels.Add(item);

            serviceCategoryModels.Add(serviceCategoryModel);

            totalAmount += itemAmount;
          }
        }
      }

      reservationCnfModel.ServiceCategories = serviceCategoryModels;
      reservationCnfModel.GrossAmount = totalAmount;

      Session["ReservationConfirmationVM"] = reservationCnfModel;

      return View(reservationCnfModel);
    }

    public ActionResult ReserveRoom()
    {
      var reservationConfirmationVM = (ReservationConfirmationViewModel)Session["ReservationConfirmationVM"];

      ReservationModel reservationModel = new ReservationModel();

      reservationModel.ClientId = Session["ClientId"].ToString();
      reservationModel.RoomId = reservationConfirmationVM.RoomId;
      reservationModel.CheckInDate = DateTime.SpecifyKind(reservationConfirmationVM.ReservationDate, DateTimeKind.Utc);
      reservationModel.CheckInTime = DateTime.SpecifyKind(reservationConfirmationVM.FromTime, DateTimeKind.Utc);
      reservationModel.CheckOutTime = DateTime.SpecifyKind(reservationConfirmationVM.ToTime, DateTimeKind.Utc);
      reservationModel.TotalAttendees = reservationConfirmationVM.Attendees;
      reservationModel.RoomPrice = reservationConfirmationVM.RoomPricePerHour;
      reservationModel.RoomAmount = reservationConfirmationVM.RoomTotalAmount;
      reservationModel.GrossAmount = reservationConfirmationVM.GrossAmount;
      reservationModel.BookingStatus = BookingStatus.Pending;
      reservationModel.BookedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

      reservationModel.CategoryItemModels = new List<ReservationCategoryItemModel>();

      if(reservationConfirmationVM.ServiceCategories.Any())
      {
        foreach (var category in reservationConfirmationVM.ServiceCategories)
        {
          foreach (var item in category.ItemModels)
          {
            var reservCatItemModel = new ReservationCategoryItemModel();
            reservCatItemModel.CategoryId = category.Id;
            reservCatItemModel.ItemId = item.ItemId;
            reservCatItemModel.Quantity = item.Quantity.Value;
            reservCatItemModel.ItemPrice = item.Price.Value;
            reservCatItemModel.ItemAmount = item.Amount;

            reservationModel.CategoryItemModels.Add(reservCatItemModel);
          }
        }
      }

      try
      {
        _bookingRepo.InsertReservation(reservationModel);
        return RedirectToAction("index", "booking");
      }
      catch (Exception)
      {

        return RedirectToAction("Index","Error");
      }      
      
    }

  }
}