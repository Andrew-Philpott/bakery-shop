using AndysBakery.Models;
using System.Collections.Generic;

namespace AndysBakery.ViewModels
{
  public class AddFlavorToTreat
  {
    public int TreatId { get; set; }
    public List<Flavor> FlavorsList { get; set; }
  }
}