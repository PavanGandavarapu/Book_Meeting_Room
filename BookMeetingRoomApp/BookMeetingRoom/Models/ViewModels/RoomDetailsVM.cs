using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models.ViewModels
{
  public class RoomDetailsVM
  {
    public RoomModel RoomModel { get; set; }
    public RoomListVM RoomListVMs { get; set; }

    public List<ServiceCategoryModel> CategoryModels { get; set; }
  }
}