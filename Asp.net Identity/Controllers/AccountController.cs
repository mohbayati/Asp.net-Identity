using Asp.net_Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_Identity.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Account account,string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(account.UserName);
                if(user !=null)
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, account.Password, false, false);
                    if(result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }

                }
                ModelState.AddModelError(nameof(account.UserName), "is not valid");
                
            }
            return View(account);
        }
    }
}
