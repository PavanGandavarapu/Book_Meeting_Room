using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public class ClientRepos : IClientRepo
  {
    private const string Collection = CommonServices.ClientCollection;
    private DbContext _context;
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<Client> _clientCollection;
    
    public ClientRepos(DbContext dbContext)
    {
      _context = dbContext;
      _db = _context.db;
      _clientCollection = _db.GetCollection<Client>(Collection);
      
    }

    public void Delete(string id)
    {
      var filter = Builders<Client>.Filter.Eq("CLientId", id);
      var update = Builders<Client>.Update.Set("Status", false);
      _clientCollection.UpdateOne(filter, update);
    }

    public List<ClientModel> GetAll()
    {
      var clients = _clientCollection.Find(type => type.Status == true).ToList();
      List<ClientModel> clientModels = new List<ClientModel>();
      foreach (var client in clients)
      {
        var clientModel = new ClientModel(client);
        clientModels.Add(clientModel);
      }
      return clientModels;
    }

    public ClientModel GetById(string id)
    {
      var client = _clientCollection.Find(type => type.ClientId == id && type.Status == true).FirstOrDefault();
      if (client == null)
      {
        return null;
      }

      var ClientModel = new ClientModel(client);

      return ClientModel;
    }

    public bool Insert(ClientModel model, out string clientId)
    {
      var client = new Client(model);
      _clientCollection.InsertOne(client);
      clientId = client.ClientId;
      return true;
    }

    public bool Update(ClientModel model)
    {
      var client = new Client(model);
      var filter = Builders<Client>.Filter.Eq(b => b.ClientId, model.ClientId);
      var result = _clientCollection.ReplaceOne(filter, client, new ReplaceOptions { IsUpsert = true });
      return true;
    }

    public bool IsEmailRegistered(string email)
    {
      return _clientCollection.Find(client=>client.Email == email).Any();
    }
  }
}