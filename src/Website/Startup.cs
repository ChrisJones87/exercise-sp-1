using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Data;
using Website.Data.Models;

namespace Website
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddControllersWithViews();

         services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                    options.LoginPath="/Authentication/Index";
                    options.LogoutPath="/Authentication/Logout";
                 });

         services.AddDbContext<WebsiteDatabaseContext>(options =>
         {
            options.UseInMemoryDatabase("Website");
         });
         //services.AddEntityFrameworkInMemoryDatabase();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         SeedDatabase(app);

         app.UseHttpsRedirection();
         app.UseStaticFiles();

         app.UseRouting();

         app.UseAuthentication();
         app.UseAuthorization();
         
         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
         });
      }

      private static void SeedDatabase(IApplicationBuilder app)
      {
         using var scope = app.ApplicationServices.CreateScope();
         
         using var context = scope.ServiceProvider.GetRequiredService<WebsiteDatabaseContext>();

         var chris = CreateUserChris();
         context.Users.Add(chris);

         context.SaveChanges();
      }

      private static User CreateUserChris()
      {
         var password = Utilities.HashPassword("secure");

         return new User
         {
            Username = "Chris",
            Name = "Christopher Jones",
            Password = password.Password,
            Salt = password.Salt
         };
      }
   }
}
