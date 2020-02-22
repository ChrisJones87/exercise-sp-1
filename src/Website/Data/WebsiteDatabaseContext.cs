using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Website.Data.Models;

namespace Website.Data
{
   public class WebsiteDatabaseContext : DbContext
   {
      public WebsiteDatabaseContext(DbContextOptions<WebsiteDatabaseContext> options) : base(options)
      {
         
      }

      public DbSet<User> Users { get; set; }
   }
}
