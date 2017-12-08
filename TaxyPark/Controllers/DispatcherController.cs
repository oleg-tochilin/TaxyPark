using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxyPark.Models;

namespace TaxyPark.Controllers
{
    public class DispatcherController : BaseController
    {
      
      public ActionResult Index()
      {
      
        return View();
      }

      
      public ActionResult EditOrder()
      {
        return View();
      }

      public JsonResult GetOrders()
      {              
        var q = from o in db.Orders
          join d in db.Users on o.DriverId equals d.Id into od
          from d in od.DefaultIfEmpty()

        
          join s in db.Statuses on o.StatusId equals s.Id into os
          from s in os.DefaultIfEmpty()

          orderby o.Id descending
        
          select new 
          {
            Id = o.Id, 
            DateCreated = o.DateCreated,
          
            o.ClientName,
            o.ClientPhoneNumber,
            o.FromPoint,
            o.ToPoint,
            DriverId = d!=null ? d.Id : 0,
            DriverName = d!=null ? d.Name : String.Empty,
            StatusId = o.StatusId,
            StatusName = s!=null ? s.Name : String.Empty,
           };
      
         return Json(q,JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetDrivers()
      {
        return Json(db.Users.Where(x=>x.Role==UserRole.Driver).OrderBy(x=>x.Name),JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetStatuses()
      {
        return Json(db.Statuses.OrderBy(x=>x.Id),JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult SetOrder(Order order)
      {
        var o = db.Orders.Find(order.Id);

        if(o!=null)
        {
          var e = db.Entry(o);
          o.FromPoint = order.FromPoint;
          o.ToPoint = order.ToPoint;

          if(o.DriverId==null && order.DriverId!=null)
          {
            o.StatusId = 2; //принят
          }
          else
          {
            o.StatusId = order.StatusId;
          }

          o.DriverId = order.DriverId;
        
        
          e.State = System.Data.Entity.EntityState.Modified;
          db.SaveChanges();

          var driver = db.Users.Find(o.DriverId);
          var status = db.Statuses.Find(o.StatusId);

          return Json(new {Id = o.Id,StatusName = status.Name, DriverName = driver!=null ? driver.Name : String.Empty});
        }

        return null;
      }
    }
}