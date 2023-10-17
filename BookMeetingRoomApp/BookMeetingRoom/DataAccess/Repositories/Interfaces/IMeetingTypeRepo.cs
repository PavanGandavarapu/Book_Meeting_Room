using BookMeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public interface IMeetingTypeRepo
  {
    bool InsertMeetingType(MeetingTypeModel model);
    bool UpdateMeetingType(MeetingTypeModel model);
    void DeleteMeetingType(string id);
    MeetingTypeModel GetMeetingTypeById(string id);
    List<MeetingTypeModel> GetAllMeetingTypes();
  }
}