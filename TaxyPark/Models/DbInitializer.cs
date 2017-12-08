using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaxyPark.Models
{
  public class DbInitializer: DropCreateDatabaseIfModelChanges<TaxyParkDbContext>
  {
    public override void InitializeDatabase(TaxyParkDbContext context)
    {
      base.InitializeDatabase(context);
    }

    
    protected override void Seed(TaxyParkDbContext context)
    {
      foreach (var s in new string[]{"новый","принят","выполняется","выполнен","отменён"})
      {
        context.Statuses.Add(new OrderStatus{Name = s});
      }
      base.Seed(context);
    }
  }
}