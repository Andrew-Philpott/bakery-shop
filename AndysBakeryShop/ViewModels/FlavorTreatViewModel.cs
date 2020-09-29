namespace AndysBakery.ViewModels
{
  public class FlavorTreatViewModel
  {
    public int FlavorTreatId { get; set; }
    public string Description { get; set; }

    public FlavorTreatViewModel(int id, string description)
    {
      FlavorTreatId = id;
      Description = description;
    }
  }
}