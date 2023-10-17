using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookMeetingRoom.Models;

namespace BookMeetingRoom.DataAccess.Entities
{
  public class ReservationCategoryItem
  {
    public ReservationCategoryItem() { }
    public ReservationCategoryItem(ReservationCategoryItemModel item)
    {
      CategoryItemId = item.CategoryItemId;
      BookingId = item.BookingId;
      CategoryId = item.CategoryId;
      ItemId = item.ItemId;
      Quantity = item.Quantity;
      ItemPrice = item.ItemPrice;
      ItemAmount = item.ItemAmount;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryItemId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string BookingId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string ItemId { get; set; }

    public int Quantity { get; set; }

    public double ItemPrice { get; set; }
    public double ItemAmount { get; set; }
  }
}