using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookMeetingRoom.Models;

namespace BookMeetingRoom.DataAccess.Entities
{
  public class User
  {
    public User() { }

    public User(UserModel user)
    {
      Id = user.Id;
      ClientId = user.ClientId;
      UserName = user.UserName;
      Password = user.Password;
      Role = user.Role;
      Status = user.Status;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string ClientId { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }

    public bool Status { get; set; }
  }
}