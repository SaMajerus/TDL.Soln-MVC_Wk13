using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
//New code:  [Ln 9]
using Microsoft.AspNetCore.Identity;  

namespace ToDoList
{
  public class Startup
  {
    public Startup(IWebHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json");
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

      services.AddEntityFrameworkMySql()
        .AddDbContext<ToDoListContext>(options => options
        .UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));

      //New code
      services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ToDoListContext>()
                .AddDefaultTokenProviders();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseDeveloperExceptionPage();

      app.UseAuthentication(); //new code  [this line]

      app.UseRouting();
      
      app.UseAuthorization(); //new code  [this line]
      
      app.UseEndpoints(routes =>
      {
        routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
      });

      app.UseStaticFiles();

      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("Hello World!");
      });
    }
  }
}