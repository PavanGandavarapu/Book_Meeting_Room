using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.DataAccess
{
  public class DbContext
  {
    private readonly string _connectionString;
    private readonly string _databaseName;
    private readonly MongoClient _client;
    public IMongoDatabase db;

    public DbContext()
    {
      _connectionString = ConfigurationManager.AppSettings["Mongo_CS"];
      _databaseName = ConfigurationManager.AppSettings["Mongo_DB"];

      _client = new MongoClient(_connectionString);
      db = _client.GetDatabase(_databaseName);
    }
  }
}