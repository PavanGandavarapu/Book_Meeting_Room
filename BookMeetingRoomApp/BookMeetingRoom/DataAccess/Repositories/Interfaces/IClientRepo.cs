using BookMeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public interface IClientRepo
  {
    bool Insert(ClientModel model, out string clientId);
    bool Update(ClientModel model);
    void Delete(string id);
    ClientModel GetById(string id);
    List<ClientModel> GetAll();

    bool IsEmailRegistered(string email);
  }
}