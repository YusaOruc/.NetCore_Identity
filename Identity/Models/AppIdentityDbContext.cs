using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class AppIdentityDbContext:IdentityDbContext<AppUser,AppRole, string>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("server=DESKTOP-PLG92B8;database=ECommerceDb2;integrated security=true;");
        //}
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext>options):base(options)
        {
           
        }
        public DbSet<Product> Products { get; set; }

    }
}
