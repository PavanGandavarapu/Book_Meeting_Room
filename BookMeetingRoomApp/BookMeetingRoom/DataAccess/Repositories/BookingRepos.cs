using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.Enums;
using BookMeetingRoom.Models;
using BookMeetingRoom.Models.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public class BookingRepos : IBookingRepo
  {
    private const string ReservationCollection = CommonServices.ReservationCollection;
    private const string CategoryItemCollection = CommonServices.ReservationCategoryItemCollection;
    private DbContext _context;
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<Reservation> _reservationCollection;
    private readonly IMongoCollection<ReservationCategoryItem> _reservationItemCollection;

    public BookingRepos(DbContext dbContext)
    {
      _context = dbContext;
      _db = _context.db;
      _reservationCollection = _db.GetCollection<Reservation>(ReservationCollection);
      _reservationItemCollection = _db.GetCollection<ReservationCategoryItem>(CategoryItemCollection);
    }

    public bool InsertReservation(ReservationModel reservationModel)
    {
      int counter = 0;
      if(!_reservationCollection.Find(_ => true).Any())
      {
        counter = 1;
      }
      else
      {
        var orderId = GetAllReservations().LastOrDefault().OrderId;
        counter = orderId + 1;
      }

      var reservation = new Reservation(reservationModel);
      reservation.OrderId = counter;      
      
      _reservationCollection.InsertOne(reservation);

      foreach(var categoryItem in reservationModel.CategoryItemModels)
      {
        categoryItem.BookingId = reservation.BookingId;
        InsertCategoryItems(categoryItem);
      }

      return true;
    }

    public bool InsertCategoryItems(ReservationCategoryItemModel reservationCategoryItemModel)
    {
      var reservationCategoryItem = new ReservationCategoryItem(reservationCategoryItemModel);
      _reservationItemCollection.InsertOne(reservationCategoryItem);
      return true;
    }

    public List<ReservationModel> GetAllReservations(string clientId = null)
    {
      List<Reservation> reservations;
      if (clientId != null)
      {
        reservations = _reservationCollection.Find(r => r.ClientId == clientId).ToList();        
      }
      else
      {
        reservations = _reservationCollection.Find(_ => true).ToList();
      }

      var reservationModels = reservations.Select(reservation => new ReservationModel(reservation)).ToList();     

      return reservationModels;
    }

    public bool IsRoomBooked(string roomId, DateTime date, DateTime checkInTime)
    {
      date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
      int checkInTimeHour = DateTime.SpecifyKind(checkInTime, DateTimeKind.Utc).Hour;
      var data = _reservationCollection.Find(r => r.RoomId == roomId && r.CheckInDate == date && (r.BookingStatus != BookingStatus.Cancelled)).FirstOrDefault();

      if(data!=null)
      {
        if (checkInTimeHour > data.CheckOutTime.Hour)
        {
          return false;
        }
        else
        {
          return true;
        }
      }      
      
      return false;
    }   

    public bool UpdateBookingStatus(string bookingId,BookingStatus status)
    {
      var update = Builders<Reservation>.Update.Set("BookingStatus", status);
      
      var result = _reservationCollection.UpdateOne(b => b.BookingId == bookingId, update);
      return result.IsAcknowledged;
    }

    public ReservationModel GetReservationByBookingId(string bookingId)
    {
      ReservationModel reservationModel = new ReservationModel();
      Reservation reservation = _reservationCollection.Find(r => r.BookingId == bookingId).FirstOrDefault();

      if(reservation!=null)
      {
        reservationModel = new ReservationModel(reservation);

        var filter = Builders<ReservationCategoryItem>.Filter.Eq(ci=>ci.BookingId,bookingId);
        List<ReservationCategoryItem> categoryItems = _reservationItemCollection.Find(i => i.BookingId == bookingId).ToList();
        var categoryItemModels = categoryItems.Select(it => new ReservationCategoryItemModel(it)).ToList();

        reservationModel.CategoryItemModels = categoryItemModels;
        //reservationModel.CategoryItemModels.AddRange(categoryItemModels);
      }      

      return reservationModel;
    }

    public bool Reschedule(RescheduleModel rescheduleModel)
    {
      var oldGrossAmount = rescheduleModel.ReservationModel.GrossAmount;
      var oldRoomAmount = rescheduleModel.ReservationModel.RoomAmount;
      var amount = oldGrossAmount - oldRoomAmount;
      TimeSpan hours = rescheduleModel.CheckOutTime - rescheduleModel.CheckInTime;

      var roomAmount = hours.TotalHours * rescheduleModel.ReservationModel.RoomPrice;
      var grossAmount = amount + roomAmount;

      rescheduleModel.ReservationDate = DateTime.SpecifyKind(rescheduleModel.ReservationDate, DateTimeKind.Utc);
      rescheduleModel.CheckInTime = DateTime.SpecifyKind(rescheduleModel.CheckInTime, DateTimeKind.Utc);
      rescheduleModel.CheckOutTime = DateTime.SpecifyKind(rescheduleModel.CheckOutTime, DateTimeKind.Utc);
      var update = Builders<Reservation>.Update
                  .Set(r => r.CheckInDate, rescheduleModel.ReservationDate)
                  .Set(r => r.CheckInTime, rescheduleModel.CheckInTime)
                  .Set(r => r.CheckOutTime, rescheduleModel.CheckOutTime)
                  .Set(r => r.RoomAmount, roomAmount)
                  .Set(r => r.GrossAmount, grossAmount)
                  .Set(r => r.BookingStatus, BookingStatus.Pending);
      var result = _reservationCollection.UpdateOne(b => b.BookingId == rescheduleModel.BookingId, update);
      return result.IsAcknowledged;
    }

  }
}