using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using React.AspNet;
using Website.Data;

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
         services.AddHttpContextAccessor();

         // Add React and V8 JS engine support
         services.AddReact();
         services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName)
                 .AddV8();

         // Add MVC controllers and views
         services.AddControllersWithViews();

         // Add cookie authentication middleware
         services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                    options.LoginPath="/Authentication/Index";
                    options.LogoutPath="/Authentication/Logout";
                 });

         // Configure EF Core and use the in-memory database for demo purposes
         services.AddDbContext<WebsiteDatabaseContext>(options =>
         {
            options.UseInMemoryDatabase("Website");
         });

         // Find all command handlers inside this assembly to register with MediatR
         services.AddMediatR(typeof(Startup).Assembly);
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

         // Set up the database and apply test data
         app.SeedDatabase();

         app.UseHttpsRedirection();

         app.UseReact(config =>{});
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

      
   }
}
