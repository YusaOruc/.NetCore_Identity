using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {

        public SignInManager<AppUser> signInManager { get; }
        public MemberController(SignInManager<AppUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Policy = "CityPolicy")]
        public IActionResult ClaimPhoneNumber()
        {
            return View();
        }
        public IActionResult LogOut()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
