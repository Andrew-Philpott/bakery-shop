using AndysBakery.Models;
using System.Collections.Generic;

namespace AndysBakery.ViewModels
{
  public class AddTreatToFlavor
  {
    public int FlavorId { get; set; }
    public List<Treat> TreatsList { get; set; }
  }
}