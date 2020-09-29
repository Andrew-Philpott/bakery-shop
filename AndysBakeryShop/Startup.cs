using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AndysBakery.Models;
using System;
using System.Threading.Tasks;

namespace AndysBakery
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddDbContext<AndysBakeryContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));

      services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AndysBakeryContext>()
        .AddDefaultTokenProviders();

      services.AddAuthorization(options =>
      {
        options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
      });

      services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 0;
      });
    }

    public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(routes =>
      {
        routes.MapControllerRoute(
              name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");
      });

      CreateAdminRole(serviceProvider).Wait();
    }
    private static async Task CreateAdminRole(IServiceProvider serviceProvider)
    {
      var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      string admin = "Admin";
      bool adminExists = await RoleManager.RoleExistsAsync(admin);
      if (!adminExists)
      {
        IdentityResult roleResult = await RoleManager.CreateAsync(new IdentityRole(admin));
      }
      ApplicationUser user = await UserManager.FindByNameAsync("admin@gmail.com");
      if (user == null)
      {
        user = new ApplicationUser()
        {
          UserName = "admin@gmail.com",
        };
        await UserManager.CreateAsync(user, "admin");
      }
      await UserManager.AddToRoleAsync(user, "Admin");
    }
  }
}