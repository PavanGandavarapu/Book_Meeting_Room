using BookMeetingRoom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class RoomModel
  {
    public RoomModel() { }

    public RoomModel(Room room)
    {
      RoomId = room.RoomId;
      Title = room.Title;
      RoomNo = room.RoomNo;
      Floor = room.Floor;
      BuildingName = room.BuildingName;
      Location = room.Location;
      Price = room.Price;
      Capacity = room.Capacity;
      RoomImageFileName = room.RoomImageFileName;
      IsActive = room.IsActive;
      MeetingType_Id = room.MeetingType_Id;
    }

    public string RoomId { get; set; }

    [Required]
    public string Title { get; set; }

    [Required,Display(Name = "Room Number")]
    public string RoomNo { get; set; }

    [Required]
    public string Floor { get; set; }

    [Required, Display(Name = "Building Name")]
    public string BuildingName { get; set; }

    [Required]
    public string Location { get; set; }

    [Required, Display(Name = "Price per hour")]
    public double? Price { get; set; }

    [Required]
    public int? Capacity { get; set; }
   
    public string RoomImageFileName { get; set; }

    [Display(Name = "Room Image")]    
    public HttpPostedFileBase RoomImage { get; set; }

    public bool IsActive { get; set; } = true;

    [Required, Display(Name = "Meeting type")]    
    public string MeetingType_Id { get; set; }

    public MeetingTypeModel MeetingTypeModel { get; set; }

    
  }
}