using BookMeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public interface IUserRepo
  {
    bool Insert(UserModel model);
    bool Update(UserModel model);
    void Delete(string id);
    UserModel GetById(string id);
    List<UserModel> GetAll();
    UserModel GetUserByUsernamePassword(LoginModel login);
    bool IsValidClient(LoginModel login);
  }
}