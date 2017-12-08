using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaxyPark.Models
{
  public class TaxyParkDbContext:DbContext
  {
    public TaxyParkDbContext(string name):base(name)
    {
    }

    public DbSet<User> Users { get; set; }
    
    
    public DbSet<Order> Orders {get;set;}
    public DbSet<OrderStatus> Statuses {get;set;}
  }
}