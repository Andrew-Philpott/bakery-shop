using System.Collections.Generic;

namespace AndysBakery.Models
{
  public class Treat
  {
    public Treat()
    {
      this.Flavors = new HashSet<FlavorTreat> { };
    }
    public int TreatId { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public virtual ICollection<FlavorTreat> Flavors { get; set; }
    public virtual ApplicationUser User { get; set; }
  }
}