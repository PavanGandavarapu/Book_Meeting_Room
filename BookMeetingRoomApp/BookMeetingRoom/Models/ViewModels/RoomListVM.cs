using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookMeetingRoom.Models.ViewModels
{
  public class RoomListVM
  {
    public List<RoomModel> RoomModels { get; set; }

    [Required, Display(Name = "Meeting Type")]
    public string MeetingTypeId { get; set; }

    [Required]
    public string Location { get; set; }

    [Required,Display(Name = "Total Attendees ")]
    public int? Capacity { get; set; }

    [Required, Display(Name = "Reservation Date")]
    public DateTime ReservationDate { get; set; }

    [Required, Display(Name = "Check-In Time")]
    [DataType(DataType.Time)]
    public DateTime FromTime { get; set; }

    [Required, Display(Name = "Check-Out Time")]
    [DataType(DataType.Time)]
    public DateTime ToTime { get; set; }


    public string Message { get; set; }


    public IEnumerable<SelectListItem> MeetingTypes { get; set; }
    public IEnumerable<SelectListItem> LocationList { get; set; }
  }
}