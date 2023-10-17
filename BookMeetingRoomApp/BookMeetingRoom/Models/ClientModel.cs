using BookMeetingRoom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Models
{
  public class ClientModel
  {
    public ClientModel() { }

    public ClientModel(Client client)
    {
      ClientId = client.ClientId;
      FirstName = client.FirstName;
      LastName = client.LastName;
      Email = client.Email;
      Phone = client.Phone;
      Organization = client.Organization;
      Status = client.Status;
    }

    public string ClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Organization { get; set; }
    public bool Status { get; set; }

    public string FullName => $"{FirstName} {LastName}";
  }
}