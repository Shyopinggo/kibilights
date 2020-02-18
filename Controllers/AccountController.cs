using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

using KibiLights.Models;
using KibiLights.ViewModels;

namespace KibiLights.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext context;

        public AccountController(ApplicationContext context)
        {
            this.context = context;
        }

        [Authorize(Roles = "Admin, User")]
        public IActionResult Test()
        {
            string role = User.FindFirst(u => u.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            return Content(User.Identity.Name + " " + role);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            User user = null;
            user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null) ModelState.AddModelError("", "This user already exists");
            if (ModelState.IsValid)
            {
                Role role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                user = new User { Email = model.Email, Password = HashPassword(model.Password), Role = role };
                context.Users.Add(user);
                await context.SaveChangesAsync();
                await Authenticate(model.Email, role.Name);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View();
            var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == HashPassword(model.Password));
            if (user == null)
            {
                ModelState.AddModelError("", "Wrong e-mail or password");
                return View();
            }
            await Authenticate(user.Email, user.Role.Name);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(string name, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private string HashPassword(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hasher = SHA512.Create())
            {
                var resultBytes = hasher.ComputeHash(bytes);
                var result = new StringBuilder();
                foreach (var b in resultBytes)
                    result.Append(b.ToString("X2"));
                return result.ToString();
            }
        }
    }
}