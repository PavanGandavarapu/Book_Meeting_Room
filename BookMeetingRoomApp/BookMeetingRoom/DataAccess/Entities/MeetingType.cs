using BookMeetingRoom.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookMeetingRoom.DataAccess.Entities
{
  public class MeetingType
  {
    public MeetingType() { }
    public MeetingType(MeetingTypeModel model)
    {
      Id = model.Id;
      Purpose = model.Purpose;
      Status = model.Status;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Purpose { get; set; }

    public bool Status { get; set; }
  }
}