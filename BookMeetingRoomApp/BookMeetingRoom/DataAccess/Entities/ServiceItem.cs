using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using BookMeetingRoom.Models;

namespace BookMeetingRoom.DataAccess.Entities
{
  public class ServiceItem
  {
    public ServiceItem() { }

    public ServiceItem(ServiceItemModel item)
    {
      ItemId = item.ItemId;
      CategoryId = item.CategoryId;
      ItemName = item.ItemName;
      Price = item.Price.Value;
      Status = item.Status;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ItemId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string  CategoryId { get; set; }

    public string ItemName { get; set; }

    public double Price { get; set; }

    public bool Status { get; set; }

  }
}