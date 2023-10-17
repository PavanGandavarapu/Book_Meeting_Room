using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class LoginModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required,Display(Name = "User Name")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }
}