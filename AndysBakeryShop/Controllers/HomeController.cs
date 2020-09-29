using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AndysBakery.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AndysBakery.Controllers
{
  public class HomeController : Controller
  {
    private readonly AndysBakeryContext _db;
    public HomeController(AndysBakeryContext db)
    {
      _db = db;
    }
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/ourstory")]
    public ActionResult OurStory()
    {
      return View();
    }

    [HttpGet("/menu")]
    public async Task<IActionResult> Menu()
    {
      List<Treat> treats = await _db.Treats.OrderBy(treat => treat.Description).ToListAsync();
      List<Flavor> flavors = await _db.Flavors.OrderBy(flavor => flavor.
      Description).ToListAsync();
      ViewBag.Treats = treats;
      ViewBag.Flavors = flavors;
      return View();
    }

    [HttpGet("/contactus")]
    public ActionResult ContactUs()
    {
      return View();
    }
  }
}