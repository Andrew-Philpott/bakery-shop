using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndysBakery.Models;
using AndysBakery.ViewModels;

namespace AndysBakery.Controllers
{
  [Authorize]
  public class OrdersController : Controller
  {
    private readonly AndysBakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public OrdersController(UserManager<ApplicationUser> userManager, AndysBakeryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [HttpGet("/orders")]
    public async Task<IActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      List<OrderViewModel> orders = await _db.Orders
      .Where(entry => entry.User.Id == currentUser.Id)
      .OrderBy(order => order.Date)
      .Select(n => new OrderViewModel { OrderId = n.OrderId, Date = n.Date }).ToListAsync();

      foreach (OrderViewModel item in orders)
      {
        List<OrderFlavorTreat> theseOrderFlavorTreats = await _db.OrderFlavorTreat.Where(orderFlavorTreat => orderFlavorTreat.OrderId == item.OrderId).ToListAsync();

        foreach (OrderFlavorTreat treat in theseOrderFlavorTreats)
        {
          item.Price = (from oft in theseOrderFlavorTreats
                        join ft in _db.FlavorTreat on oft.FlavorTreatId equals ft.FlavorTreatId
                        join f in _db.Flavors on ft.FlavorId equals f.FlavorId
                        join t in _db.Treats on ft.TreatId equals t.TreatId
                        select (t.Price * oft.Quantity)).Sum();
        }
      }
      return View(orders);
    }

    public async Task<IActionResult> Create()
    {
      IList<FlavorTreatViewModel> flavorTreats = await (from t in _db.Treats
                                                        join ft in _db.FlavorTreat on t.TreatId equals ft.TreatId
                                                        join f in _db.Flavors on ft.FlavorId equals f.FlavorId
                                                        select new FlavorTreatViewModel(ft.FlavorTreatId, (f.Description + " " + t.Description + " " + t.Price))).ToListAsync();
      return View(flavorTreats);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int FlavorTreatId, int quantity)
    {
      FlavorTreat ft = await _db.FlavorTreat.Include(x => x.Treat).SingleOrDefaultAsync(flavorTreat => flavorTreat.FlavorTreatId == FlavorTreatId);
      var price = ft.Treat.Price * quantity;
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      Order order = new Order { Price = price };
      order.User = currentUser;
      await _db.Orders.AddAsync(order);
      await _db.SaveChangesAsync();
      if (FlavorTreatId != 0)
      {
        await _db.OrderFlavorTreat.AddAsync(new OrderFlavorTreat() { OrderId = order.OrderId, FlavorTreatId = FlavorTreatId, Quantity = quantity });
      }
      await _db.SaveChangesAsync();

      return RedirectToAction("Details", new { id = order.OrderId });
    }

    public async Task<IActionResult> Details(int id)
    {
      Order order = await _db.Orders.SingleOrDefaultAsync(orders => orders.OrderId == id);
      List<OrderFlavorTreat> theseOrderFlavorTreats = await _db.OrderFlavorTreat.Where(orderFlavorTreat => orderFlavorTreat.OrderId == id).ToListAsync();

      ViewBag.OrderDetails = from oft in theseOrderFlavorTreats
                             join ft in _db.FlavorTreat on oft.FlavorTreatId equals ft.FlavorTreatId
                             join f in _db.Flavors on ft.FlavorId equals f.FlavorId
                             join t in _db.Treats on ft.TreatId equals t.TreatId
                             select new OrderDetailsViewModel { OrderFlavorTreatId = oft.OrderFlavorTreatId, Quantity = oft.Quantity, Description = (f.Description + " " + t.Description), Price = t.Price };
      foreach (OrderDetailsViewModel item in ViewBag.OrderDetails)
      {
        order.Price += item.Price;
      }
      return View(order);
    }

    public async Task<IActionResult> Edit(int id)
    {
      Order thisOrder = await _db.Orders.SingleOrDefaultAsync(orders => orders.OrderId == id);
      IEnumerable<FlavorTreatViewModel> flavorTreats = from t in _db.Treats
                                                       join ft in _db.FlavorTreat on t.TreatId equals
ft.TreatId
                                                       join f in _db.Flavors on ft.FlavorId equals f.FlavorId
                                                       select new FlavorTreatViewModel(ft.FlavorTreatId, (f.Description + " " + t.Description + " " + t.Price));

      ViewBag.FlavorTreats = flavorTreats;
      return View(thisOrder);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, int FlavorTreatId, int quantity)
    {
      Order order = await _db.Orders.SingleOrDefaultAsync();
      FlavorTreat ft = await _db.FlavorTreat.SingleOrDefaultAsync(flavorTreat => flavorTreat.FlavorTreatId == FlavorTreatId);

      var treat = await _db.Treats.SingleOrDefaultAsync(treat => treat.TreatId == ft.TreatId);
      double priceOfTreat = treat.Price;
      order.Price = (quantity * priceOfTreat);

      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      order.User = currentUser;
      _db.Entry(order).State = EntityState.Modified;
      await _db.SaveChangesAsync();

      if (FlavorTreatId != 0)
      {
        await _db.OrderFlavorTreat.AddAsync(new OrderFlavorTreat() { OrderId = order.OrderId, FlavorTreatId = FlavorTreatId, Quantity = quantity });
      }
      await _db.SaveChangesAsync();

      return RedirectToAction("Details", new { id = order.OrderId });
    }

    public async Task<IActionResult> Delete(int id)
    {
      Order order = await _db.Orders.SingleOrDefaultAsync(orders => orders.OrderId == id);
      List<OrderFlavorTreat> theseOrderFlavorTreats = await _db.OrderFlavorTreat.Where(orderFlavorTreat => orderFlavorTreat.OrderId == id).ToListAsync();

      ViewBag.OrderDetails = (from oft in theseOrderFlavorTreats
                              join ft in _db.FlavorTreat on oft.FlavorTreatId equals ft.FlavorTreatId
                              join f in _db.Flavors on ft.FlavorId equals f.FlavorId
                              join t in _db.Treats on ft.TreatId equals t.TreatId
                              select new OrderDetailsViewModel { Quantity = oft.Quantity, Description = (f.Description + " " + t.Description), Price = t.Price }).ToList();
      return View(order);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      Order order = await _db.Orders.SingleOrDefaultAsync(orders => orders.OrderId == id);
      _db.Orders.Remove(order);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    [HttpPost, ActionName("DeleteItem")]
    public async Task<IActionResult> DeleteItem(int orderId, int orderFlavorTreatId)
    {
      OrderFlavorTreat thisOrderFlavorTreat = await _db.OrderFlavorTreat.SingleOrDefaultAsync(orderFlavorTreat => orderFlavorTreat.OrderFlavorTreatId == orderFlavorTreatId);
      _db.OrderFlavorTreat.Remove(thisOrderFlavorTreat);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }
  }
}