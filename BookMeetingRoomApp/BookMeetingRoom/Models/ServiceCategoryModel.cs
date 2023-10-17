using BookMeetingRoom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class ServiceCategoryModel
  {
    public ServiceCategoryModel() { }

    public ServiceCategoryModel(ServiceCategory category)
    {
      Id = category.Id;
      CategoryName = category.CategoryName;
      Status = category.Status;
    }

    public string Id { get; set; }

    [Required, Display(Name = "Service Category")]
    public string CategoryName { get; set; }
    public bool Status { get; set; } = true;

    public List<ServiceItemModel> ItemModels { get; set; }
  }
}