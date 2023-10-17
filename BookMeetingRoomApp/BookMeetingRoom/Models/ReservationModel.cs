using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class ReservationModel
  {
    public ReservationModel()
    {

    }
    public ReservationModel(Reservation reservation)
    {
      BookingId = reservation.BookingId;
      ClientId = reservation.ClientId;
      RoomId = reservation.RoomId;
      CheckInDate = reservation.CheckInDate;
      CheckInTime = reservation.CheckInTime;
      CheckOutTime = reservation.CheckOutTime;
      TotalAttendees = reservation.TotalAttendees;
      RoomPrice = reservation.RoomPrice;
      RoomAmount = reservation.RoomAmount;
      GrossAmount = reservation.GrossAmount;
      BookedDate = reservation.BookedDate;
      BookingStatus = reservation.BookingStatus;
      OrderId = reservation.OrderId;
    }
    public string BookingId { get; set; }

    public string ClientId { get; set; }

    public string RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime CheckOutTime { get; set; }
    public int TotalAttendees { get; set; }
    public double RoomPrice { get; set; }
    public double RoomAmount { get; set; }

    public double GrossAmount { get; set; }

    public DateTime BookedDate { get; set; }

    public BookingStatus BookingStatus { get; set; }

    public int OrderId { get; set; }

    public List<ReservationCategoryItemModel> CategoryItemModels { get; set; }

    public ClientModel ClientModel { get; set; }
    public RoomModel RoomModel { get; set; }
  }
}