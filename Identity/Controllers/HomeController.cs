using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {


        public UserManager<AppUser> userManager { get; }
        public SignInManager<AppUser> signInManager { get; }

        protected readonly AppIdentityDbContext _context;
        public HomeController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, AppIdentityDbContext context)
        {
            this._context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            
        }
        public IActionResult Index()
        {
            var values = _context.Products.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult SingUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SingUp(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                //AppUser appUser = new AppUser();

                //appUser.UserName = user.UserName;
                //appUser.Email = user.Email;
                //appUser.PhoneNumber = user.PhoneNumber;
                AppUser appUser = user.Adapt<AppUser>();
                IdentityResult result = userManager.CreateAsync(appUser,user.Password).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("LogIn");
                }
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View(user);
        }
        [HttpGet]
        public IActionResult LogIn(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null)
                {
                    if(await userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabıız bir süreliğine kilitlenmiştir.Lütfen daha sonra tekrar deneyiniz.");
                        return View(loginViewModel);
                    }
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result= await signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);

                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);
                        return RedirectToAction("Index", "Member");
                        if (TempData["ReturnUrl"] != null)
                        {
                            return RedirectToAction(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index","Member");
                    }
                    else
                    {
                        await userManager.AccessFailedAsync(user);
                        int fail = await userManager.GetAccessFailedCountAsync(user);
                        ModelState.AddModelError("", $"{fail} başarısız giriş.Geriye {3-fail} hakkınız kaldı.");
                        if (fail == 3)
                        {
                            await userManager.SetLockoutEndDateAsync(user, new System.DateTimeOffset(DateTime.Now.AddMinutes(20)));
                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolayı 20 dakika kitlenmiştir.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Geçersiz email veya şifre.");
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz email veya şifre.");
                }
            }
            return View(loginViewModel);
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;
            if (user != null)
            {

                string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;
                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme);

                Helper.PasswordReset.PAsswordResetSendEmail(passwordResetLink, "h.yusa98@gmail.com",user.UserName.ToString());
                ViewBag.status = "successful";
            }
            else
            {
                ModelState.AddModelError("", "Sistemde kayıtlı email adresi bulunamamıştır.");
            }
            return View(passwordResetViewModel);
        }
        public IActionResult ResetPasswordConfirm(string userId,string token) 
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("Password")]PasswordResetViewModel passwordResetViewModel)
        {
            string userId = TempData["userId"].ToString();
            string token=TempData["token"].ToString();

            AppUser user = userManager.FindByIdAsync(userId).Result;
            if (user != null)
            {
                IdentityResult result =await userManager.ResetPasswordAsync(user,token,passwordResetViewModel.Password);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    TempData["PasswordResetInfo"] = "Şifreniz başarıyla yenilenmiştir.Yeni şifreniz ile giriş yapabilirsiniz.";
                    ViewBag.status = "success";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Bir hata meyadana gelmiştir. Lütfen daha sonra tekrar deneyiniz.");
            }
            return View();
        }
    }
}
