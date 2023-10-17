using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess;
using BookMeetingRoom.DataAccess.Repositories;
using BookMeetingRoom.Enums;
using BookMeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS = BookMeetingRoom.Common.CommonServices;

namespace BookMeetingRoom.Controllers
{
  [SessionExpireFilter]
  public class BookingController : Controller
  {
    private readonly IBookingRepo _bookingRepo;
    private readonly IClientRepo _clientRepo;
    private readonly IRoomRepo _roomRepo;
    private readonly IServiceCategoryRepo _catRepo;
    public BookingController()
    {
      var context = new DbContext();
      _bookingRepo = new BookingRepos(context);
      _clientRepo = new ClientRepos(context);
      _roomRepo = new RoomRepos(context);
      _catRepo = new ServiceCategoryRepos(context);
    }

    // GET: Booking
    public ActionResult Index(string clientId = null)
    {
      List<ReservationModel> reservationModels;
      if ((bool)Session[CS.IsAdmin])
      {
        if(clientId != null)
        {
          reservationModels = _bookingRepo.GetAllReservations(clientId);
        }
        else
        {
          reservationModels = _bookingRepo.GetAllReservations();
        }
      }
      else
      {
        clientId = Session[CS.ClientId].ToString();
        reservationModels = _bookingRepo.GetAllReservations(clientId);
      }

      foreach (var reservationModel in reservationModels)
      {
        reservationModel.ClientModel = _clientRepo.GetById(reservationModel.ClientId);
        reservationModel.RoomModel = _roomRepo.GetById(reservationModel.RoomId);
      }

      ViewBag.Alert = TempData["Alert"];

      return View(reservationModels.OrderByDescending(r=>r.BookingId).OrderBy(r=>r.BookingStatus));
    }

    [Route("booking/approve/{bookingId}")]
    public ActionResult Approve(string bookingId)
    {
      if (_bookingRepo.UpdateBookingStatus(bookingId, BookingStatus.Approved))
      {
        TempData["Alert"] = CS.Alert(Alerts.Success, "Approved successfully");
      }
      else
      {
        TempData["Alert"] = CS.Alert(Alerts.Warning, "Error approving reservation");
      }
      return RedirectToAction(nameof(Index));
    }

    [Route("booking/cancel/{bookingId}")]
    public ActionResult Cancel(string bookingId)
    {
      if (_bookingRepo.UpdateBookingStatus(bookingId,BookingStatus.Cancelled))
      {
        TempData["Alert"] = CS.Alert(Alerts.Success, "Cancelled successfully");
      }
      else
      {
        TempData["Alert"] = CS.Alert(Alerts.Warning, "Error approving reservation");
      }
      return RedirectToAction(nameof(Index));
    }

    [Route("booking/details/{bookingId}")]
    public ActionResult Details(string bookingId)
    {
      var reservationModel = _bookingRepo.GetReservationByBookingId(bookingId);
      reservationModel.ClientModel = _clientRepo.GetById(reservationModel.ClientId);
      reservationModel.RoomModel = _roomRepo.GetById(reservationModel.RoomId);

      reservationModel.CategoryItemModels.ForEach(item => { 
        item.CategoryModel = _catRepo.GetById(item.CategoryId);
        item.ItemModel = _catRepo.GetByItemId(item.ItemId);
      });

      return View(reservationModel);
    }

    [Route("booking/re-schedule/{bookingId}")]
    public ActionResult Reschedule(string bookingId)
    {
      
      RescheduleModel rescheduleModel = new RescheduleModel
      {
        BookingId = bookingId,
      };
      return View(rescheduleModel);
    }

    [Route("booking/re-schedule/{bookingId}")]
    [HttpPost]
    public ActionResult Reschedule(string bookingId, RescheduleModel rescheduleModel)
    {
      var reservationModel = _bookingRepo.GetReservationByBookingId(bookingId);

      if(_bookingRepo.IsRoomBooked(reservationModel.RoomId, rescheduleModel.ReservationDate,rescheduleModel.CheckInTime))
      {
        rescheduleModel.AvailabilityMsg = $"Sorry!, The room is booked for the given date {rescheduleModel.ReservationDate.ToString("MM-dd-yyyy")}";
      }
      else
      {
        rescheduleModel.ReservationModel = reservationModel;
        if (_bookingRepo.Reschedule(rescheduleModel))
        {
          rescheduleModel.AvailabilityMsg = $"Your booking has been rescheduled on {rescheduleModel.ReservationDate.ToString("MM-dd-yyyy")}";
        }
        else
        {
          rescheduleModel.AvailabilityMsg = $"Sorry!, The room is booked for the given date {rescheduleModel.ReservationDate.ToString("MM-dd-yyyy")}";
        }
      }
      return View(rescheduleModel);
    }

  }
}