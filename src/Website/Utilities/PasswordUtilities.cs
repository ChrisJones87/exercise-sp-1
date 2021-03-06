using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Website.Utilities
{
   public static class PasswordUtilities
   {
      /// <summary>
      /// Generates a 128 bit salt for hashing passwords securely
      /// </summary>
      public static byte[] GeneratePasswordSalt()
      {
         using var randomNumberGenerator = new RNGCryptoServiceProvider();

         byte[] salt = new byte[128 / 8];
         randomNumberGenerator.GetBytes(salt);

         return salt;
      }

      public static (string Password, byte[] Salt) HashPassword(string password)
      {
         if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", nameof(password));

         var salt = GeneratePasswordSalt();

         var hashedPasswordBytes = KeyDerivation.Pbkdf2(password: password,
                                                        salt: salt,
                                                        prf: KeyDerivationPrf.HMACSHA1,
                                                        iterationCount: 10000,
                                                        numBytesRequested: 256 / 8);

         var hashedPassword = Convert.ToBase64String(hashedPasswordBytes);

         return (hashedPassword, salt);
      }

      public static string HashPassword(string password, byte[] salt)
      {
         if (password == null) throw new ArgumentNullException(nameof(password));
         if (salt == null) throw new ArgumentNullException(nameof(salt));

         var hashedPasswordBytes = KeyDerivation.Pbkdf2(password: password,
                                                        salt: salt,
                                                        prf: KeyDerivationPrf.HMACSHA1,
                                                        iterationCount: 10000, // TODO: This should be stored in the database with the user
                                                        numBytesRequested: 256 / 8);

         var hashedPassword = Convert.ToBase64String(hashedPasswordBytes);

         return hashedPassword;
      }
   }
}