using System;
using System.Collections.Generic;

namespace AndysBakery.Models
{
  public class Order
  {
    public Order()
    {
      this.OrderFlavorTreat = new HashSet<OrderFlavorTreat> { };
    }
    public int OrderId { get; set; }
    public double Price { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public virtual ICollection<OrderFlavorTreat> OrderFlavorTreat { get; set; }
    public virtual ApplicationUser User { get; set; }
  }
}