using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using KibiLights.Models;
using KibiLights.ViewModels;

namespace KibiLights.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext context;
        private readonly IStringLocalizer<AccountController> localizer;

        public AccountController(ApplicationContext context, IStringLocalizer<AccountController> localizer)
        {
            this.context = context;
            this.localizer = localizer;
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
            if (user != null) ModelState.AddModelError("", localizer["UserExists"]);
            if (ModelState.IsValid)
            {
                Role role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                user = new User { Email = model.Email, Password = Tools.HashPassword(model.Password), Role = role };
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
            var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == Tools.HashPassword(model.Password));
            if (user == null)
            {
                ModelState.AddModelError("", localizer["WrongPassword"]);
                return View();
            }
            await Authenticate(user.Email, user.Role.Name);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetToken(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password)) return BadRequest("Name or password is empty");
            var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == name && u.Password == Tools.HashPassword(password));
            if (user == null) return Unauthorized("Wrong login or password.");
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            var response = new { access_token = encodedToken, username = identity.Name};
            return Json(response);
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
    }
}