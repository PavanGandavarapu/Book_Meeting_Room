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
  public class ServiceCategoryRepos : IServiceCategoryRepo
  {
    private const string CategoryCollection = CommonServices.ServiceCategoriesCollection;
    private const string ItemCollection = CommonServices.CategoryItemCollection;
    private DbContext _context;
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<ServiceCategory> _categoryCollection;
    private readonly IMongoCollection<ServiceItem> _itemCollection;

    public ServiceCategoryRepos(DbContext dbContext)
    {
      _context = dbContext;
      _db = _context.db;
      _categoryCollection = _db.GetCollection<ServiceCategory>(CategoryCollection);
      _itemCollection = _db.GetCollection<ServiceItem>(ItemCollection);
    }

    public bool Insert(ServiceCategoryModel model)
    {
      try
      {
        var serviceCategory = new ServiceCategory(model);
        _categoryCollection.InsertOne(serviceCategory);
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool Update(ServiceCategoryModel model)
    {
      var serviceCategory = new ServiceCategory(model);
      var filter = Builders<ServiceCategory>.Filter.Eq(b => b.Id, model.Id);
      var result = _categoryCollection.ReplaceOne(filter, serviceCategory, new ReplaceOptions { IsUpsert = true });
      return true;
    }

    public void Delete(string id)
    {
      var filter = Builders<ServiceCategory>.Filter.Eq("Id", id);
      var update = Builders<ServiceCategory>.Update.Set("Status", false);
      _categoryCollection.UpdateOne(filter, update);
    }

    public ServiceCategoryModel GetById(string id)
    {
      try
      {
        var ServiceCategory = _categoryCollection.Find(type => type.Id == id).FirstOrDefault();
        if (ServiceCategory == null)
        {
          return null;
        }
        return new ServiceCategoryModel(ServiceCategory);
      }
      catch (Exception ex)
      {        
        throw new Exception(ex.Message);
      }
        
    }

    public List<ServiceCategoryModel> GetAll()
    {
      var serviceCategories = _categoryCollection.Find(type => type.Status == true).ToList();
      List<ServiceCategoryModel> serviceCategoryModels = new List<ServiceCategoryModel>();
      foreach (var serviceCategory in serviceCategories)
      {
        var serviceCategoryModel = new ServiceCategoryModel(serviceCategory);
        serviceCategoryModels.Add(serviceCategoryModel);
      }
      return serviceCategoryModels;
    }

    public List<ServiceCategoryModel> GetAllCategoriesWithItems()
    {
      var serviceCategories = _categoryCollection.Find(type => type.Status == true).ToList();
      List<ServiceCategoryModel> serviceCategoryModels = new List<ServiceCategoryModel>();
      foreach (var serviceCategory in serviceCategories)
      {
        var serviceCategoryModel = new ServiceCategoryModel(serviceCategory);
        serviceCategoryModel.ItemModels = GetAllItems(serviceCategoryModel.Id);
        serviceCategoryModels.Add(serviceCategoryModel);
      }
      return serviceCategoryModels;
    }


    public bool InsertItem(ServiceItemModel model)
    {
      var serviceItem = new ServiceItem(model);
      _itemCollection.InsertOne(serviceItem);
      return true;
    }

    public bool UpdateItem(ServiceItemModel model)
    {
      var serviceItem = new ServiceItem(model);
      var filter = Builders<ServiceItem>.Filter.Eq(b => b.ItemId, model.ItemId);
      var result = _itemCollection.ReplaceOne(filter, serviceItem, new ReplaceOptions { IsUpsert = true });
      return true;
    }

    public void DeleteItem(string id)
    {
      var filter = Builders<ServiceItem>.Filter.Eq("ItemId", id);
      var update = Builders<ServiceItem>.Update.Set("Status", false);
      _itemCollection.UpdateOne(filter, update);
    }

    public ServiceItemModel GetByItemId(string id)
    {
      var serviceItem = _itemCollection.Find(type => type.ItemId == id).FirstOrDefault();
      if (serviceItem == null)
      {
        return null;
      }
      return new ServiceItemModel(serviceItem);

    }

    public List<ServiceItemModel> GetAllItems(string categoryId)
    {
      var serviceItems = _itemCollection.Find(type => type.CategoryId == categoryId && type.Status == true)
                                              .ToList();
      List<ServiceItemModel> serviceItemModels = new List<ServiceItemModel>();
      foreach (var serviceItem in serviceItems)
      {
        var ServiceItemModel = new ServiceItemModel(serviceItem);
        serviceItemModels.Add(ServiceItemModel);
      }
      return serviceItemModels;
    }

  }
}