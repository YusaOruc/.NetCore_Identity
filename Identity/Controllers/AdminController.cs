using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager { get; }
        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var values = userManager.Users.ToList();
            return View(values);
        }
    }
}
