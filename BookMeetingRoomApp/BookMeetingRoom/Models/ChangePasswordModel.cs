using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class ChangePasswordModel
  {
    public string UserId { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required, Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
  }
}