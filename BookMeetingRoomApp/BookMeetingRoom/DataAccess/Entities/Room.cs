using BookMeetingRoom.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookMeetingRoom.DataAccess.Entities
{
  public class Room
  {
    public Room() { }

    public Room(RoomModel room)
    {
      RoomId = room.RoomId;
      Title = room.Title;
      RoomNo = room.RoomNo;
      Floor = room.Floor;
      BuildingName = room.BuildingName;
      Location = room.Location;
      Price = room.Price.Value;
      Capacity = room.Capacity.Value;
      RoomImageFileName = room.RoomImageFileName;
      IsActive = room.IsActive;
      MeetingType_Id = room.MeetingType_Id;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string RoomId { get; set; }
    public string Title { get; set; }
    public string RoomNo { get; set; }
    public string Floor { get; set; }
    public string BuildingName { get; set; }
    public string Location { get; set; }
    public double Price { get; set; }
    public int Capacity { get; set; }
    public string RoomImageFileName { get; set; }
    public bool IsActive { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string MeetingType_Id { get; set; }
  }
}