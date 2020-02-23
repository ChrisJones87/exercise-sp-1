using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Website.Commands;
using Website.Models;

namespace Website.Controllers
{
   public class AuthenticationController : Controller
   {
      private readonly ILogger _logger;
      private readonly IMediator _mediator;

      public AuthenticationController(ILogger<AuthenticationController> logger, IMediator mediator)
      {
         _logger = logger;
         _mediator = mediator;
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

         var command = new SignInCommand(login.Username, login.Password, login.RememberMe);
         var success = await _mediator.Send(command, cancellationToken);

         if (!success)
         {
            ModelState.AddModelError("password", "Your username or password is incorrect");
            return View("Index", login);
         }

         return RedirectToActionPermanent("Index", "Home");
      }

      public async Task<IActionResult> Logout(CancellationToken cancellationToken)
      {
         var command = new SignOutCommand();
         await _mediator.Send(command, cancellationToken);
         
         return RedirectToAction("Index", "Home");
      }
   }
}
