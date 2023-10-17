using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Enums
{
  public enum BookingStatus
  {
    Pending = 1,
    Approved,
    Cancelled,
    Rescheduled
  }
}