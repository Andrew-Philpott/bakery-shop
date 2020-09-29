using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AndysBakery.Models;
using AndysBakery.ViewModels;

namespace AndysBakery.Controllers
{
  public class AccountController : Controller
  {
    private readonly AndysBakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, AndysBakeryContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _roleManager = roleManager;
      _db = db;
    }
    [Authorize]
    [Route("/account")]
    public IActionResult Index()
    {
      return View();
    }
    [Route("/register")]
    public IActionResult Register()
    {
      return View();
    }
    [Route("/login")]
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost("/login")]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index", "Account");
      }
      else
      {
        return View();
      }
    }

    [HttpPost("/register")]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      var user = new ApplicationUser { UserName = model.Email };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction("Login", "Account");
      }
      else
      {
        return View();
      }
    }

    [HttpPost]
    [Route("/account")]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }
  }
}