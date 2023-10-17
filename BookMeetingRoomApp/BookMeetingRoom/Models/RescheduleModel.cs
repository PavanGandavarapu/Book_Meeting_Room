using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class RescheduleModel
  {
    [Required,Display(Name = "Reschedule Date")]
    [DataType(DataType.Date)]
    public DateTime ReservationDate { get; set; }

    [Required, Display(Name = "Check-In Time")]
    [DataType(DataType.Time)]
    public DateTime CheckInTime { get; set; }

    [Required, Display(Name = "Check-Out Time")]
    [DataType(DataType.Time)]
    public DateTime CheckOutTime { get; set; }

    [Required]
    public string BookingId { get; set; }

    public ReservationModel ReservationModel { get; set; }

    public string AvailabilityMsg { get; set; }
  }
}