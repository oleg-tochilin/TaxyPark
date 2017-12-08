using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxyPark.Models;

namespace TaxyPark.Controllers
{
    public class AdminController : BaseController
    {
      public ActionResult Index()
      {
        return View();
      }

       public ActionResult EditUser()
      {
        return View();
      }

      public JsonResult GetUsers()
      {
        var q = from u in db.Users          
          orderby u.Id descending
          select new { u.Id,u.Name,u.Login,Role = u.Role.ToString()};

        return Json(q,JsonRequestBehavior.AllowGet);
      }


      public JsonResult GetRoles()
      {
        return Json(Enum.GetNames(typeof(UserRole)).Select(x=>new {Name = x}),JsonRequestBehavior.AllowGet);
      }


      [HttpPost]
      public JsonResult SetUser(User user)
      {
        var u = db.Users.Find(user.Id);
        if(u!=null)
        { 
          u.Role = user.Role;
          u.Name = user.Name;

          db.Entry(u).State = System.Data.Entity.EntityState.Modified;

          db.SaveChanges();

          return Json(new { u.Id,u.Name,u.Login,Role = u.Role.ToString()});
        }

        return null;
      }
    }
}