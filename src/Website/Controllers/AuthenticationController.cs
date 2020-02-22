using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Website.Data;
using Website.Data.Models;
using Website.Models;

namespace Website.Controllers
{
   public class AuthenticationController : Controller
   {
      private readonly ILogger _logger;
      private readonly WebsiteDatabaseContext _context;

      public AuthenticationController(ILogger<AuthenticationController> logger, WebsiteDatabaseContext context)
      {
         _logger = logger;
         _context = context;
      }

      public IActionResult Index()
      {
         return View(new LoginViewModel());
      }

      public async Task<IActionResult> LoginAsync(LoginViewModel login, CancellationToken cancellationToken)
      {
         if (!ModelState.IsValid)
         {
            return View("Index", login);
         }

         var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == login.Username, cancellationToken);

         if (user == null)
         {
            ModelState.AddModelError("password", "Your username or password is incorrect");
            return View("Index", login);
         }

         var hashedPassword = Utilities.HashPassword(login.Password, user.Salt);

         if (user.Password != hashedPassword)
         {
            ModelState.AddModelError("password", "Your username or password is incorrect");
            return View("Index", login);
         }

         await SignInAsync(user, login.RememberMe);

         return RedirectToActionPermanent("Index", "Home");
      }

      private async Task SignInAsync(User user, bool persistent)
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("FullName", user.Name),
            new Claim(ClaimTypes.Role, "Administrator"),
         };

         var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

         var authProperties = new AuthenticationProperties
         {
            // 60 minute login
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),

            // Persist the cookie after the browser closes
            IsPersistent = persistent,

            IssuedUtc = DateTime.UtcNow,

            RedirectUri = "/"
         };

         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                       new ClaimsPrincipal(claimsIdentity),
                                       authProperties);
      }

      public async Task<IActionResult> Logout()
      {
         await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
         return RedirectToAction("Index", "Home");
      }
   }
}
