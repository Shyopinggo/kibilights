using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using KibiLights.Models;
using KibiLights.Models.Lighthouse;

namespace KibiLights.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LighthouseController : Controller
    {
        private readonly ApplicationContext context;

        public LighthouseController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var facilities = context.Facilities.Include(f => f.City).Include(f => f.City.Country).ToList();
            return View(facilities);
        }

        [HttpGet]
        public IActionResult AddFacility()
        {
            ViewData["Cities"] = context.Cities.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddFacility(string name, int cityId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Content("Name is empty!");
            }
            var city = context.Cities.FirstOrDefault(c => c.Id == cityId);
            var facility = new Facility { Name = name, City = city};
            context.Facilities.Add(facility);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFacility(int id)
        {
            Facility facility = context.Facilities.FirstOrDefault(f => f.Id == id);
            if (facility != null)
            {
                context.Facilities.Remove(facility);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return Content("Facility id was null");
        }
    }
}