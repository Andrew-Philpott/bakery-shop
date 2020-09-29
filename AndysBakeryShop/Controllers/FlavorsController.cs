using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AndysBakery.Models;
using AndysBakery.ViewModels;

namespace AndysBakery.Controllers
{
  [Authorize(Roles = "Admin")]
  public class FlavorsController : Controller
  {
    private readonly AndysBakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public FlavorsController(UserManager<ApplicationUser> userManager, AndysBakeryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    [HttpGet("/flavors")]
    public async Task<IActionResult> Index()
    {
      List<Flavor> flavors = await _db.Flavors.OrderBy(flavor => flavor.Description).ToListAsync();
      return View(flavors);
    }

    public async Task<IActionResult> Create()
    {
      var treats = await _db.Treats.ToListAsync();
      ViewBag.TreatId = new SelectList(treats, "TreatId", "Description");
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Flavor flavor)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      flavor.User = currentUser;
      await _db.Flavors.AddAsync(flavor);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
      Flavor thisFlavor = await _db.Flavors.AsQueryable()
        .Include(flavor => flavor.Treats)
        .ThenInclude(join => join.Treat)
        .SingleOrDefaultAsync(flavor => flavor.FlavorId == id);

      List<Treat> treats = thisFlavor.Treats
        .Select(treat => treat.Treat)
        .OrderBy(treat => treat.Description)
        .ToList();

      ViewBag.Treats = treats;
      return View(thisFlavor);
    }

    public async Task<IActionResult> Edit(int id)
    {
      Flavor thisFlavor = await _db.Flavors.SingleOrDefaultAsync(flavors => flavors.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Flavor flavor)
    {
      _db.Entry(flavor).State = EntityState.Modified;
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    public async Task<IActionResult> AddTreat(int id)
    {
      Flavor flavor = await _db.Flavors.SingleOrDefaultAsync(flavors => flavors.FlavorId == id);
      var treats = await _db.Treats
        .Select(n => n)
        .ToListAsync();
      AddTreatToFlavor vm = new AddTreatToFlavor() { FlavorId = flavor.FlavorId, TreatsList = treats };
      return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> AddTreat(int FlavorId, int TreatId)
    {
      if (TreatId != 0)
      {
        await _db.FlavorTreat.AddAsync(new FlavorTreat() { FlavorId = FlavorId, TreatId = TreatId });
        await _db.SaveChangesAsync();
      }

      return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
      Flavor thisFlavor = await _db.Flavors.SingleOrDefaultAsync(flavors => flavors.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      Flavor thisFlavor = await _db.Flavors.SingleOrDefaultAsync(flavors => flavors.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }
  }
}