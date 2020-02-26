using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using KibiLights.Models;

namespace KibiLights.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationContext context;

        public UsersController(ApplicationContext context)
        {
            this.context = context;
        }
        
        public IActionResult Index()
        {
            var users = context.Users.Include(u => u.Role).ToList();
            ViewData["Roles"] = context.Roles.ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult AddUser(string email, string password, int roleId)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return BadRequest("Email or password is empty");
            var role = context.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null) return BadRequest($"Wrong roleId {roleId}");
            var user = new User { Email = email, Password = Tools.HashPassword(password), Role = role};
            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteUser(int id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return BadRequest($"User not found. id={id}");
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}