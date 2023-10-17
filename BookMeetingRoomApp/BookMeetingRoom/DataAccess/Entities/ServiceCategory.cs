using BookMeetingRoom.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookMeetingRoom.DataAccess.Entities
{
  public class ServiceCategory
  {
    public ServiceCategory() { }
    public ServiceCategory(ServiceCategoryModel model)
    {
      Id = model.Id;
      CategoryName = model.CategoryName;
      Status = model.Status;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string CategoryName { get; set; }
    public bool Status { get; set; }
  }
}