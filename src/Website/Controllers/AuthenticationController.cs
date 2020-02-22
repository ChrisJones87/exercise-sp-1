using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Models;

namespace Website.Controllers
{
   public class AuthenticationController : Controller
   {
      private readonly ILogger _logger;

      public AuthenticationController(ILogger<AuthenticationController> logger)
      {
         _logger = logger;
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

         if(login.Username != "Chris" || login.Password != "secure")
         {
            ModelState.AddModelError("password", "Your username or password is incorrect");
            return View("Index", login);
         }

         await SignInAsync(login);

         return RedirectToActionPermanent("Index", "Home");
      }

      private async Task SignInAsync(LoginViewModel login)
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Name, login.Username),
            new Claim("FullName", "Mr Fake User"),
            new Claim(ClaimTypes.Role, "Administrator"),
         };

         var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

         var authProperties = new AuthenticationProperties
         {
            // 60 minute login
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),

            // Persist the cookie after the browser closes
            IsPersistent = login.RememberMe,

            IssuedUtc = DateTime.UtcNow,

            RedirectUri = "/"
         };

         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                       new ClaimsPrincipal(claimsIdentity),
                                       authProperties);
      }
   }
}
