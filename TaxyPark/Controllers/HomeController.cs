using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxyPark.Models;

namespace TaxyPark.Controllers
{
  public class HomeController : BaseController
  {
    [AllowAnonymous]
    public ActionResult Index(string id)
    {
      //первый запуск или нет админа
      if(!db.Users.Any(x=>x.Role==UserRole.Admin))
      {
        return RedirectToAction("Register","Account");
      }


      var user = GetAuthUser();
      if(user!=null)
      {
        if(user.Role==UserRole.Dispatcher)
        {
          return RedirectToAction("Index","Dispatcher");
        }
        else if(user.Role==UserRole.Driver)
        {
          return RedirectToAction("Index","Driver");
        }
        else if(user.Role==UserRole.Admin)
        {
          return RedirectToAction("Index","Admin");
        }
        else if(user.Role==UserRole.New)
        {
          return RedirectToAction("NotConfirmed","Home");
        }
      }

      else
      {
        if(!String.IsNullOrEmpty(id))
        {
          var order = db.Orders.Where(x=>x.ExternalId == id).FirstOrDefault();

          if(order!=null)
          {
            ViewBag.StatusName = db.Statuses.Find(order.StatusId).Name;

            if(order.DriverId !=null && order.StatusId!=5)
            {
              ViewBag.DriverName = db.Users.Find(order.DriverId).Name;
            }
            ViewBag.Order = order;

            return View("ShowOrder");
          }
        }

         return View();
      }

       return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public void CancelOrder(Order order)
    {
      var o = db.Orders.Where(x=>x.ExternalId == order.ExternalId).FirstOrDefault();

      if(o!=null)
      {
        o.StatusId = 5;
        db.Entry(o).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();
      } 
    }

    [AllowAnonymous]
    [HttpPost]
    public JsonResult CreateOrder(Order order)
    {
      db.Orders.Add(order);
      db.SaveChanges();

      db.Entry(order).Reload();

      return Json(order);
    }
   
  }
}