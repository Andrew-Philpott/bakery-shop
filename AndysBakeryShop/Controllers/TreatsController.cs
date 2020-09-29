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
  public class TreatsController : Controller
  {
    private readonly AndysBakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public TreatsController(UserManager<ApplicationUser> userManager, AndysBakeryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    [AllowAnonymous]
    [HttpGet("/treats")]
    public ActionResult Index()
    {
      List<Treat> userTreats = _db.Treats.OrderBy(treat => treat.Description).ToList();
      return View(userTreats);
    }
    public ActionResult Create()
    {
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Description");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;
      _db.Treats.Add(treat);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
        .Include(treat => treat.Flavors)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);

      List<Flavor> flavors = thisTreat.Flavors
        .Select(flavor => flavor.Flavor)
        .OrderBy(flavor => flavor.Description)
        .ToList();

      ViewBag.Flavors = flavors;
      return View(thisTreat);
    }

    public async Task<IActionResult> Edit(int id)
    {
      var thisTreat = await _db.Treats.SingleOrDefaultAsync(treats => treats.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Treat treat)
    {
      _db.Entry(treat).State = EntityState.Modified;
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    public async Task<IActionResult> AddFlavor(int id)
    {
      Treat treat = await _db.Treats.SingleOrDefaultAsync(treats => treats.TreatId == id);
      AddFlavorToTreat viewModel = new AddFlavorToTreat();
      viewModel.TreatId = treat.TreatId;
      viewModel.FlavorsList = await _db.Flavors.ToListAsync();
      return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddFlavor(int TreatId, int FlavorId)
    {
      if (FlavorId != 0)
      {
        await _db.FlavorTreat.AddAsync(new FlavorTreat() { TreatId = TreatId, FlavorId = FlavorId });
        await _db.SaveChangesAsync();
      }

      return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
      Treat thisTreat = await _db.Treats.SingleOrDefaultAsync(treats => treats.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      Treat thisTreat = await _db.Treats.SingleOrDefaultAsync(treats => treats.TreatId == id);
      _db.Treats.Remove(thisTreat);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }
  }
}