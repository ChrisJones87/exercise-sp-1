using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Data.Models;
using Website.Utilities;

namespace Website.Commands
{
   public sealed class SignInCommandHandler : IRequestHandler<SignInCommand, bool>
   {
      private WebsiteDatabaseContext _context;
      private readonly IHttpContextAccessor _httpContextAccessor;

      public SignInCommandHandler(WebsiteDatabaseContext context, IHttpContextAccessor httpContextAccessor)
      {
         _context = context;
         _httpContextAccessor = httpContextAccessor;
      }

      public async Task<bool> Handle(SignInCommand command, CancellationToken cancellationToken)
      {
         var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == command.Username, cancellationToken);

         if (user == null)
         {
            return false;
         }

         var hashedPassword = PasswordUtilities.HashPassword(command.Password, user.Salt);

         if (user.Password != hashedPassword)
         {
            return false;
         }

         await SignInAsync(user, command.IsPersistent);
         return true;
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

         await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                            new ClaimsPrincipal(claimsIdentity),
                                                            authProperties);
      }
   }
}