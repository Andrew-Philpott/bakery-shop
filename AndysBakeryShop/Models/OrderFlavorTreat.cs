namespace AndysBakery.Models
{
  public class OrderFlavorTreat
  {
    public int OrderFlavorTreatId { get; set; }
    public int Quantity { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int FlavorTreatId { get; set; }
    public FlavorTreat FlavorTreat { get; set; }

  }
}