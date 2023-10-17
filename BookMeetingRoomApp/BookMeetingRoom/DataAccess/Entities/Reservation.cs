using BookMeetingRoom.Enums;
using BookMeetingRoom.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.DataAccess.Entities
{
  public class Reservation
  {
    public Reservation()
    {

    }

    public Reservation(ReservationModel reservation)
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

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string BookingId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string ClientId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
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

  }
}