using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaxyPark.Models;

namespace TaxyPark.Controllers
{
  [Authorize]
  public class BaseController:Controller
  {
    protected TaxyParkDbContext db = new TaxyParkDbContext("DefaultConnection");

    protected User GetAuthUser()
    {
      if(!User.Identity.IsAuthenticated)
        return null;


      return db.Users.Where(x=>x.Login == User.Identity.Name).FirstOrDefault();
    }
  }
}