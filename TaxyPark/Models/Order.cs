using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaxyPark.Models
{

  public class OrderStatus:BaseRecord
  {
    public string Name{get;set;}
  }

  public class Order:BaseRecord
  {
    [Required]
    public string ExternalId {get;set;} = Guid.NewGuid().ToString("N");

    public DateTime DateCreated{get;set;} = DateTime.Now;

    public string ClientName {get;set;}

    [Required]
    public string ClientPhoneNumber {get;set;}

    public int? DriverId{get;set;}
    
    public int? DispatcherId{get;set;}

    [DefaultValue(1)]
    public int StatusId{get;set;} = 1;

    public string FromPoint {get;set;}
    public string ToPoint {get;set;}
  }
}