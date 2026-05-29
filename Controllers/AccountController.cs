using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simulation1.Models;
using Simulation1.ViewModels;
using Simulation1.ViewModels.Account;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Simulation1.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            AppUser user = new AppUser()
            {
                UserName = registerVM.UserName,
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(registerVM);
                }
            }

            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction(nameof(Login));

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            AppUser? user = await _userManager.FindByEmailAsync(loginVM.Email);

            if (user is null)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,false,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid");
                return View(loginVM);
            }
            return RedirectToAction("Index", "Home");
        
    }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            return View();
        }
    }
}
