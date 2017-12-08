using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxyPark.Models
{
  public enum UserRole:int
  {
    New = 0,
    Driver = 1,
    Dispatcher = 2,
    Admin = 3
  }

  public class User:BaseRecord
  {
    public string Login {get;set;}
    public string Name {get;set;}
    public string Password { get;set;}
    public UserRole Role {get;set;}
      
  }
}