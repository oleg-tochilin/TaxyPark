using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaxyPark.Models
{
  public class UserLogin
  {
    public string Login { get; set; }
    public string Password { get; set; }
  }
 
  public class UserRegister
  {
    public string Login { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
 
    public string ConfirmPassword { get; set; }
  }
}