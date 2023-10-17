using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public class MeetingTypeRepos : IMeetingTypeRepo
  {
    private const string Collection = CommonServices.MeetingTypeCollection;
    private DbContext _context;
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<MeetingType> _typeCollection;

    public MeetingTypeRepos(DbContext dbContext)
    {
      _context = dbContext;
      _db = _context.db;
      _typeCollection = _db.GetCollection<MeetingType>(Collection);
    }

    public bool InsertMeetingType(MeetingTypeModel model)
    {
      try
      {
        var meetingType = new MeetingType(model);
        _typeCollection.InsertOne(meetingType);
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool UpdateMeetingType(MeetingTypeModel model)
    {
      var meetingType = new MeetingType(model);
      var filter = Builders<MeetingType>.Filter.Eq(b => b.Id, model.Id);
      var result = _typeCollection.ReplaceOne(filter, meetingType, new ReplaceOptions { IsUpsert = true });
      return true;
    }

    public void DeleteMeetingType(string id)
    {
      var filter = Builders<MeetingType>.Filter.Eq("Id", id);
      var update = Builders<MeetingType>.Update.Set("Status", false);
      _typeCollection.UpdateOne(filter, update);
    }

    public MeetingTypeModel GetMeetingTypeById(string id)
    {
      try
      {
        var meetingType = _typeCollection.Find(type => type.Id == id).FirstOrDefault();
        if (meetingType == null)
        {
          return null;
        }
        return new MeetingTypeModel(meetingType);
      }
      catch (Exception ex)
      {        
        throw new Exception(ex.Message);
      }
        
    }

    public List<MeetingTypeModel> GetAllMeetingTypes()
    {
      var meetingTypes = _typeCollection.Find(type => type.Status == true).ToList();
      List<MeetingTypeModel> meetingTypeModels = new List<MeetingTypeModel>();
      foreach (var meetingType in meetingTypes)
      {
        var meetingTypeModel = new MeetingTypeModel(meetingType);
        meetingTypeModels.Add(meetingTypeModel);
      }
      return meetingTypeModels;
    }
  }
}