using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webapp.Data;
using webapp.Models;
using webapp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }



        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            // checking for emailadress and password is not null before we get the user
            if (string.IsNullOrEmpty(loginViewModel.EmailAddress) || string.IsNullOrEmpty(loginViewModel.Password))
            {
                if (string.IsNullOrEmpty(loginViewModel.EmailAddress))
                {
                    ModelState.AddModelError(string.Empty, "Email is required.");
                }
                if (string.IsNullOrEmpty(loginViewModel.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password is required.");
                }
                return View(loginViewModel);
            }

            // gets the user by emailAddress
            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                // user is found, check password
                var passwodCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if(passwodCheck)
                {
                    // password correct, sign in 
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }
                // password is not correct
                TempData["Error"] = "Wrong credentials. Plase, try again.";
                return View(loginViewModel);
            }
            // user not found
            TempData["Error"] = "Wrong credentials. Please, try again.";
            return View(loginViewModel);

        }



        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task <IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            // checking for emailadress and password is not null before we get the user
            if (string.IsNullOrEmpty(registerViewModel.EmailAddress) || string.IsNullOrEmpty(registerViewModel.Password))
            {
                if (string.IsNullOrEmpty(registerViewModel.EmailAddress))
                {
                    ModelState.AddModelError(string.Empty, "Email is required.");
                }
                if (string.IsNullOrEmpty(registerViewModel.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password is required.");
                }
                return View(registerViewModel);
            }

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);

            // if user is not null, then there is already a user and then we dont wanna create another one
            // then we returning the view with the viewmodel
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                // Redirecting to the Index action of Home controller
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Handle errors
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerViewModel);
            }

        }



        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}

