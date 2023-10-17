using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.Models;
using BookMeetingRoom.Models.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public class RoomRepos : IRoomRepo
  {
    private const string RoomCollection = CommonServices.RoomCollection;
    private DbContext _context;
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<Room> _roomCollection;

    private readonly IMeetingTypeRepo _meetingTypeRepo;
    private readonly IBookingRepo _bookingRepo;
    public RoomRepos(DbContext dbContext)
    {
      _context = dbContext;
      _db = _context.db;
      _roomCollection = _db.GetCollection<Room>(RoomCollection);

      _meetingTypeRepo = new MeetingTypeRepos(_context);
      _bookingRepo = new BookingRepos(_context);
    }

    public void Delete(string roomId)
    {
      var filter = Builders<Room>.Filter.Eq("RoomId", roomId);
      var update = Builders<Room>.Update.Set("IsActive", false);
      _roomCollection.UpdateOne(filter, update);
    }

    public List<RoomModel> GetAll()
    {
      var rooms = _roomCollection.Find(type => type.IsActive == true).ToList();
      List<RoomModel> roomModels = new List<RoomModel>();
      foreach (var room in rooms)
      {
        var roomModel = new RoomModel(room);
        roomModel.MeetingTypeModel = _meetingTypeRepo.GetMeetingTypeById(room.MeetingType_Id);
        roomModels.Add(roomModel);
      }
      return roomModels;
    }

    public RoomModel GetById(string id)
    {
      var room = _roomCollection.Find(type => type.RoomId == id && type.IsActive == true).FirstOrDefault();
      if (room == null)
      {
        return null;
      }

      var roomModel = new RoomModel(room);
      roomModel.MeetingTypeModel = _meetingTypeRepo.GetMeetingTypeById(room.MeetingType_Id);

      return roomModel;
    }

    public bool Insert(RoomModel model)
    {
      var room = new Room(model);
      _roomCollection.InsertOne(room);
      return true;
    }

    public bool Update(RoomModel model)
    {
      var room = new Room(model);
      var filter = Builders<Room>.Filter.Eq(b => b.RoomId, model.RoomId);
      var result = _roomCollection.ReplaceOne(filter, room, new ReplaceOptions { IsUpsert = true });
      return true;
    }

   
    public List<string> GetAllRoomLocations()
    {
      var filter = Builders<Room>.Filter.Eq("IsActive", "true");
      var projection = Builders<Room>.Projection.Include("Location").Exclude("_id");
      var result = _roomCollection.Find<Room>(filter).Project(projection).ToList();

      List<string> locationList = new List<string>();
      foreach (var document in result)
      {
        string location = document.Select(el => el.Value).First().ToString();
        if (!locationList.Contains(location))
        {
          locationList.Add(location);
        }          
      }

      return locationList;
    }

    public List<RoomModel> GetUnReservedRooms(RoomListVM roomListVM)
    {
      var builder = Builders<Room>.Filter;
      var filter = builder.And(
                          builder.Eq(room => room.MeetingType_Id, roomListVM.MeetingTypeId),
                          builder.Eq(room => room.Location, roomListVM.Location),
                          builder.Gte(room => room.Capacity, roomListVM.Capacity)
                    );
      List<Room> rooms = _roomCollection.Find(filter).ToList();

      List<RoomModel> roomModels = new List<RoomModel>();
      foreach (var room in rooms)
      {
        if(!_bookingRepo.IsRoomBooked(room.RoomId, roomListVM.ReservationDate,roomListVM.FromTime))
        {
          var roomModel = new RoomModel(room);
          roomModel.MeetingTypeModel = _meetingTypeRepo.GetMeetingTypeById(room.MeetingType_Id);
          roomModels.Add(roomModel);
        }        
      }

      return roomModels;
    }
  }
}