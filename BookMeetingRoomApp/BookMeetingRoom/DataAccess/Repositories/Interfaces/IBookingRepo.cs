using BookMeetingRoom.Enums;
using BookMeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public interface IBookingRepo
  {
    bool InsertReservation(ReservationModel reservationModel);

    bool InsertCategoryItems(ReservationCategoryItemModel reservationCategoryItemModel);
    List<ReservationModel> GetAllReservations(string clientId = null);

    bool IsRoomBooked(string roomId, DateTime date, DateTime checkInTime);
    bool UpdateBookingStatus(string bookingId, BookingStatus status);

    ReservationModel GetReservationByBookingId(string bookingId);

    bool Reschedule(RescheduleModel rescheduleModel);
  }
}