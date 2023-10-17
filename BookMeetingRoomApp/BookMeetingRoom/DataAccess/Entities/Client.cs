using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using BookMeetingRoom.Models;

namespace BookMeetingRoom.DataAccess.Entities
{
  public class Client
  {
    public Client() { }

    public Client(ClientModel client)
    {
      ClientId = client.ClientId;
      FirstName = client.FirstName;
      LastName = client.LastName;
      Email = client.Email;
      Phone = client.Phone;
      Organization = client.Organization;
      Status = client.Status;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Organization { get; set; }
    public bool Status { get; set; }
  }
}