using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace BookMeetingRoom.Models
{
  public class RegisterModel
  {
    [Required,Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required, Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required, Display(Name = "Email"), EmailAddress]
    [Remote("IsEmailExist", "Account", ErrorMessage = "EMail already registered")]
    public string Email { get; set; }

    [Required, Display(Name = "Contact Number")]
    public string Phone { get; set; }

    [Display(Name = "Organization Name")]
    public string Organization { get; set; }

    [Required,Display(Name = "Password")]
    public string Password { get; set; }

    [Required, Display(Name = "Re-Type Password")]
    [Compare(nameof(Password),ErrorMessage = "Password and Confirm Password doesn't match")]
    public string ConfirmPassword { get; set; }
  }
}