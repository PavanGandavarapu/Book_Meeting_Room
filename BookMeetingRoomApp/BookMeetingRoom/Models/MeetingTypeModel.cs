using BookMeetingRoom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class MeetingTypeModel
  {
    public MeetingTypeModel() { }

    public MeetingTypeModel(MeetingType type)
    {
      Id = type.Id;
      Purpose = type.Purpose;
      Status = type.Status;
    }

    public string Id { get; set; }

    [Required, Display(Name = "Meeting Purpose")]
    public string Purpose { get; set; }
    public bool Status { get; set; } = true;
  }
}