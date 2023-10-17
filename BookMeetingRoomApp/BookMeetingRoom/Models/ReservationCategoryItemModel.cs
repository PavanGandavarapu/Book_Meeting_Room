using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookMeetingRoom.DataAccess.Entities;

namespace BookMeetingRoom.Models
{
  public class ReservationCategoryItemModel
  {
    public ReservationCategoryItemModel()
    {

    }
    public ReservationCategoryItemModel(ReservationCategoryItem item)
    {
      CategoryItemId = item.CategoryItemId;
      BookingId = item.BookingId;
      CategoryId = item.CategoryId;
      ItemId = item.ItemId;
      Quantity = item.Quantity;
      ItemPrice = item.ItemPrice;
      ItemAmount = item.ItemAmount;
    }

    public string CategoryItemId { get; set; }

    public string BookingId { get; set; }

    public string CategoryId { get; set; }

    public string ItemId { get; set; }
    public int Quantity { get; set; }

    public double ItemPrice { get; set; }
    public double ItemAmount { get; set; }

    public ServiceCategoryModel CategoryModel { get; set; }
    public ServiceItemModel ItemModel { get; set; }
  }
}
