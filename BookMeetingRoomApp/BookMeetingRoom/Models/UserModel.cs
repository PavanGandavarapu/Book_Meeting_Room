using BookMeetingRoom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class UserModel
  {
    public UserModel()
    {

    }

    public UserModel(User user)
    {
      Id = user?.Id;
      ClientId = user.ClientId;
      UserName = user.UserName;
      Password = user.Password;
      Role = user.Role;
      Status = user.Status;
    }

    public string Id { get; set; }

    public string ClientId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool Status { get; set; } = true;
  }
}