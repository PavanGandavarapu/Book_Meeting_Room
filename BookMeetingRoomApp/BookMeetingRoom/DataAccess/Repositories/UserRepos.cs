using BookMeetingRoom.Common;
using BookMeetingRoom.DataAccess.Entities;
using BookMeetingRoom.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public class UserRepos : IUserRepo
  {
    private const string Collection = CommonServices.UserCollection;
    private DbContext _context;
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<User> _userCollection;
    public UserRepos(DbContext dbContext)
    {
      _context = dbContext;
      _db = _context.db;
      _userCollection = _db.GetCollection<User>(Collection);
    }

    public void Delete(string id)
    {
      throw new NotImplementedException();
    }

    public List<UserModel> GetAll()
    {
      var users = _userCollection.Find(type => type.Status == true).ToList();
      List<UserModel> userModels = new List<UserModel>();
      foreach (var user in users)
      {
        var userModel = new UserModel(user);
        userModels.Add(userModel);
      }
      return userModels;
    }

    public UserModel GetById(string id)
    {
      var user = _userCollection.Find(type => type.Id == id && type.Status == true).FirstOrDefault();
      if (user == null)
      {
        return null;
      }

      var userModel = new UserModel(user);

      return userModel;
    }

    public bool Insert(UserModel model)
    {
      var user = new User(model);
      _userCollection.InsertOne(user);
      return true;
    }

    public bool Update(UserModel model)
    {
      var user = new User(model);
      var filter = Builders<User>.Filter.Eq(b => b.Id, model.Id);
      var result = _userCollection.ReplaceOne(filter, user, new ReplaceOptions { IsUpsert = true });
      return true;
    }

    public UserModel GetUserByUsernamePassword(LoginModel login)
    {
      User user = _userCollection.Find(u => u.UserName == login.Email && u.Password == login.Password && u.Status == true) .FirstOrDefault();

      if (user == null) return null;

      return new UserModel(user);
    }

    public bool IsValidClient(LoginModel login)
    {
      return _userCollection.Find(user=>user.UserName == login.Email && user.Password == login.Password).Any();     
    }
  }
}