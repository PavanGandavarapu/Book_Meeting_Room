using BookMeetingRoom.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookMeetingRoom.Models
{
  public class ServiceItemModel
  {
    public ServiceItemModel() { }

    public ServiceItemModel(ServiceItem item)
    {
      ItemId = item.ItemId;
      CategoryId = item.CategoryId;
      ItemName = item.ItemName;
      Price = item.Price;
      Status = item.Status;
    }

    public string ItemId { get; set; }

    [Required]
    public string CategoryId { get; set; }

    [Required,Display(Name = "Item Name")]
    public string ItemName { get; set; }

    [Required]
    public double? Price { get; set; }

    public bool Status { get; set; } = true;

    public bool IsSelected { get; set; }

    public int? Quantity { get; set; }

    public double Amount { get; set; }

    public ServiceCategoryModel ServiceCategoryModel { get; set; }
  }
}