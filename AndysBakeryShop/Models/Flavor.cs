using System.Collections.Generic;

namespace AndysBakery.Models
{
  public class Flavor
  {
    public Flavor()
    {
      this.Treats = new HashSet<FlavorTreat> { };
    }
    public int FlavorId { get; set; }
    public string Description { get; set; }
    public virtual ICollection<FlavorTreat> Treats { get; set; }
    public virtual ApplicationUser User { get; set; }
  }
}