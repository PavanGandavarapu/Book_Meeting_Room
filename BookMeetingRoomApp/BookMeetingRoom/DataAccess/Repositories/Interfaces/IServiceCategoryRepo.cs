using BookMeetingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.DataAccess.Repositories
{
  public interface IServiceCategoryRepo
  {
    bool Insert(ServiceCategoryModel model);
    bool Update(ServiceCategoryModel model);
    void Delete(string id);
    ServiceCategoryModel GetById(string id);
    List<ServiceCategoryModel> GetAll();

    List<ServiceCategoryModel> GetAllCategoriesWithItems();

    bool InsertItem(ServiceItemModel model);
    bool UpdateItem(ServiceItemModel model);
    void DeleteItem(string id);
    ServiceItemModel GetByItemId(string id);
    List<ServiceItemModel> GetAllItems(string categoryId);
  }
}