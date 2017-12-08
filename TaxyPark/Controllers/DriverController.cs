using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxyPark.Models;

namespace TaxyPark.Controllers
{
    public class DriverController : BaseController
    {
      public ActionResult EditOrder()
      {
        return View();
      }

      public JsonResult GetOrders()
      {   
        var user = GetAuthUser();
        var q = from o in db.Orders
         
          join s in db.Statuses on o.StatusId equals s.Id into os
          from s in os.DefaultIfEmpty()

          where o.DriverId == user.Id
          orderby o.Id descending
        
          select new 
          {
            Id = o.Id, 
            DateCreated = o.DateCreated,
          
            o.ClientName,
            o.ClientPhoneNumber,
            o.FromPoint,
            o.ToPoint,
            StatusId = o.StatusId,
            StatusName = s!=null ? s.Name : String.Empty,
           };
      
         return Json(q,JsonRequestBehavior.AllowGet);
      }


      [HttpPost]
      public JsonResult GetStatuses(Order order)
      {
        var o = db.Orders.Find(order.Id);
        if(o==null)
          return null;

        return Json(db.Statuses.Where(x=>x.Id > 2 && x.Id >= o.StatusId).OrderBy(x=>x.Id),JsonRequestBehavior.AllowGet);
      }


      [HttpPost]
      public JsonResult SetOrder(Order order)
      {
        var o = db.Orders.Find(order.Id);
        if(o!=null)
        {
          var e = db.Entry(o);


          o.StatusId = order.StatusId;
          e.State = System.Data.Entity.EntityState.Modified;
          db.SaveChanges();

          
          var status = db.Statuses.Find(o.StatusId);

          return Json(new {Id = o.Id,StatusName = status.Name, StatusId = o.StatusId});
        }

        return null;
      }

      public ActionResult Index()
      {
        return View();
      }
    }
}