using Example.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Respitory.Example.Models;

namespace Respitory.Example.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private IUserContext<ApplicationUser> workContext;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserContext<ApplicationUser> workContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.workContext = workContext;
        }
        public IActionResult Login(string returnUrl)
        {
            if (signInManager.IsSignedIn(User))
            {
                var cuser = workContext.GetCurrentUserAsync().Result;
                if (cuser == null)
                {
                    return RedirectToAction(nameof(Logout));
                }
                return RedirectToAction("Index", "Home");
            }
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {

            var user = await userManager.FindByEmailAsync(login.Email);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, login.Password, false, false);
                if (result.Succeeded)
                {
                    if (login.ReturnUrl == null)
                    {
                        return Redirect("/Home/Index");
                    }
                    return Redirect(login.ReturnUrl);
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("IncorrectInput", "Email is not verified.");
                    return View(login);
                }

            }
            ModelState.AddModelError("IncorrectInput", "User not found!");
            return View(login);
        }
        public IActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel login)
        {

            var user = await userManager.FindByEmailAsync(login.Email);
            if (user != null)
                return View();
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = login.Email,
                UserName = login.Email,
                PhoneNumber = "2345678909876",

            };
            await userManager.CreateAsync(applicationUser, login.Password);
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }
        
    }

}
