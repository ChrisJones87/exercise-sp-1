using System.Linq;
using MediatR;

namespace Website.Commands
{
   public class SignInCommand : IRequest<bool>
   {
      public string Username { get; }
      public string Password { get; }
      public bool IsPersistent { get; }

      public SignInCommand(string username, string password, bool isPersistent)
      {
         Username = username;
         Password = password;
         IsPersistent = isPersistent;
      }
   }
}
