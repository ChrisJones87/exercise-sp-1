using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Website.Data;

namespace Website.Commands
{
   public sealed class SignOutCommandHandler : IRequestHandler<SignOutCommand, bool>
   {
      private WebsiteDatabaseContext _context;
      private readonly IHttpContextAccessor _httpContextAccessor;

      public SignOutCommandHandler(WebsiteDatabaseContext context, IHttpContextAccessor httpContextAccessor)
      {
         _context = context;
         _httpContextAccessor = httpContextAccessor;
      }

      public async Task<bool> Handle(SignOutCommand request, CancellationToken cancellationToken)
      {
         await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
         return true;
      }
   }
}