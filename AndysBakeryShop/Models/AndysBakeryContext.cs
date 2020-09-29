using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AndysBakery.Models
{
  public class AndysBakeryContext : IdentityDbContext<ApplicationUser>
  {
    public virtual DbSet<Flavor> Flavors { get; set; }
    public virtual DbSet<Treat> Treats { get; set; }
    public virtual DbSet<FlavorTreat> FlavorTreat { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderFlavorTreat> OrderFlavorTreat { get; set; }

    public AndysBakeryContext(DbContextOptions options) : base(options)
    {
    }
  }
}