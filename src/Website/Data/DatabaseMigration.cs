using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Website.Data.Models;
using Website.Utilities;

namespace Website.Data
{
   public static class DatabaseMigration
   {
      public static void SeedDatabase(this IApplicationBuilder app)
      {
         using var scope = app.ApplicationServices.CreateScope();

         using var context = scope.ServiceProvider.GetRequiredService<WebsiteDatabaseContext>();

         var chris = CreateUserChris();
         var test = CreateUserTest();

         context.Users.Add(chris);
         context.Users.Add(test);

         context.SaveChanges();
      }

      private static User CreateUserChris()
      {
         var password = PasswordUtilities.HashPassword("secure");

         return new User
         {
            Username = "Chris",
            Name = "Christopher Jones",
            Password = password.Password,
            Salt = password.Salt
         };
      }

      private static User CreateUserTest()
      {
         var password = PasswordUtilities.HashPassword("test");

         return new User
         {
            Username = "Test",
            Name = "Test User",
            Password = password.Password,
            Salt = password.Salt
         };
      }
   }
}