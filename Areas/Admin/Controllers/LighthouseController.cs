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

        public IActionResult Facility(int id)
        {
            var facility = context.Facilities.FirstOrDefault(f => f.Id == id);
            if (facility == null) return BadRequest();
            ViewData["Facility"] = facility;
            ViewData["Beacons"] = context.Beacons.Where(b => b.Facility == facility).ToList();
            ViewData["Routes"] = context.Routes.Where(r => r.Facility == facility).ToList();
            return View();
        }

        public IActionResult AddBeacon(string name, int facilityId)
        {
            var facility = context.Facilities.FirstOrDefault(f => f.Id == facilityId);
            if (string.IsNullOrEmpty(name) || facility == null) return BadRequest("Empty name or wrong facility id");
            var beacon = new Beacon { Name = name, Facility = facility};
            context.Beacons.Add(beacon);
            context.SaveChanges();
            return RedirectToAction("Facility", new { id = facilityId});
        }

        public IActionResult DeleteBeacon(int id, int facilityId)
        {
            var beacon = context.Beacons.FirstOrDefault(b => b.Id == id);
            if (beacon == null) return BadRequest("Wrong beacon id");
            context.Beacons.Remove(beacon);
            context.SaveChanges();
            return RedirectToAction("Facility", new { id = facilityId});
        }
    }
}