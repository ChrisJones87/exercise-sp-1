using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
   public class LoginViewModel
   {
      [Required]
      public string Username { get;set;} = "";

      public string Password { get;set;} = "";

      public bool RememberMe { get;set;} = false;
   }
}
