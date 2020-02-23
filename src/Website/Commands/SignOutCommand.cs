using MediatR;

namespace Website.Commands
{
   public class SignOutCommand : IRequest<bool>
   {
      public SignOutCommand()
      {
    
      }
   }
}