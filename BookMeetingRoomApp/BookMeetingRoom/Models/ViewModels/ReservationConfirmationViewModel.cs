using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models.ViewModels
{
  public class ReservationConfirmationViewModel
  {
    public string RoomId { get; set; }
    public string RoomTitle { get; set; }
    public int Attendees { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
    public double RoomPricePerHour { get; set; }
    public string TotalHours { get; set; }
    public double RoomTotalAmount { get; set; }
    public double GrossAmount { get; set; }

    public List<ServiceCategoryModel> ServiceCategories { get; set; }
  }
}