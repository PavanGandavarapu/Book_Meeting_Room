using BookMeetingRoom.Models;
using BookMeetingRoom.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public interface IRoomRepo
  {
    bool Insert(RoomModel model);
    bool Update(RoomModel model);
    void Delete(string id);
    RoomModel GetById(string id);
    List<RoomModel> GetAll();
    List<string> GetAllRoomLocations();
    List<RoomModel> GetUnReservedRooms(RoomListVM roomListVM);
  }
}