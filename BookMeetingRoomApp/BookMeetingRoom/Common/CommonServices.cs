using BookMeetingRoom.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMeetingRoom.Common
{
  public static class CommonServices
  {
    public const string RoomCollection = "Rooms";
    public const string MeetingTypeCollection = "Meeting_Types";
    public const string ServiceCategoriesCollection = "Service_Categories";
    public const string CategoryItemCollection = "Category_Items";
    public const string ClientCollection = "Clients";
    public const string UserCollection = "Users";
    public const string ReservationCollection = "Reservation_Rooms";
    public const string ReservationCategoryItemCollection = "Reservation_Category_Items";

    //Session Variables
    public const string IsLoggedIn = "IsLoggedIn";
    public const string IsAdmin = "IsAdmin";
    public const string UserName = "UserName";
    public const string ClientId = "ClientId";
    public const string UserId = "UserId";

    public static string Alert(Alerts obj, string message)
    {
      string alertDiv = null;
      switch (obj)
      {
        case Alerts.Success:
          alertDiv = "<div class='position-fixed p-3 w-50' style='z-index: 5; bottom: 0; right: 0;'><div class='alert alert-success alert - dismissible fade show text-center' role='alert'><strong> Success! </strong>" + message + "<button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div></div>";
          break;
        case Alerts.Danger:
          alertDiv = "<div class='position-fixed p-3 w-50' style='z-index: 5; bottom: 0; right: 0;'><div class='alert alert-danger alert - dismissible fade show text-center' role='alert'><strong> Warning! </strong>" + message + "<button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div></div>";
          break;
        case Alerts.Info:
          alertDiv = "<div class='position-fixed p-3 w-50' style='z-index: 5; bottom: 0; right: 0;'><div class='alert alert-info alert - dismissible fade show text-center' role='alert'><strong> Note! </strong>" + message + "<button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div></div>";
          break;
        case Alerts.Warning:
          alertDiv = "<div class='position-fixed p-3 w-50' style='z-index: 5; bottom: 0; right: 0;'><div class='alert alert-warning alert - dismissible fade show text-center' role='alert'><strong> Warning! </strong>" + message + "<button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div></div>";
          break;
      }
      return alertDiv;
    }
  }
}