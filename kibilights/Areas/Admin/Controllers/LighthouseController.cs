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
            var facilities = context.Facilities.Include(f => f.City).Include(f => f.City.Country).Include(f => f.User).ToList();
            return View(facilities);
        }

        [HttpGet]
        public IActionResult AddFacility()
        {
            ViewData["Cities"] = context.Cities.ToList();
            ViewData["Users"] = context.Users.Include(u => u.Role).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddFacility(string name, int cityId, int userId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name is empty!");
            }
            var city = context.Cities.FirstOrDefault(c => c.Id == cityId);
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            var facility = new Facility { Name = name, City = city, User = user};
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
            var facility = context.Facilities.Include(f => f.User).FirstOrDefault(f => f.Id == id);
            if (facility == null) return BadRequest($"Wrong facility id {id}");
            ViewData["Facility"] = facility;
            ViewData["Beacons"] = context.Beacons.Where(b => b.Facility == facility).ToList();
            ViewData["Routes"] = context.Routes.Where(r => r.Facility == facility).ToList();
            return View();
        }

        public IActionResult AddBeacon(string name, int facilityId, string description)
        {
            var facility = context.Facilities.FirstOrDefault(f => f.Id == facilityId);
            if (string.IsNullOrEmpty(name) || facility == null) return BadRequest("Empty name or wrong facility id");
            var beacon = new Beacon { Name = name, Facility = facility, Description = description};
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

        public IActionResult AddRoute(string name, string content, int facilityId)
        {
            var facility = context.Facilities.FirstOrDefault(f => f.Id == facilityId);
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(content) || facility == null) return BadRequest("Empty or wrong value for name, content, or facilityId");
            var route = new Route { Name = name, Content = content, Facility = facility};
            context.Routes.Add(route);
            context.SaveChanges();
            return RedirectToAction("Facility", new { id = facilityId});
        }

        public IActionResult DeleteRoute(int id, int facilityId)
        {
            var route = context.Routes.FirstOrDefault(r => r.Id == id);
            if (route == null) return BadRequest("Wrong route id");
            context.Routes.Remove(route);
            context.SaveChanges();
            return RedirectToAction("Facility", new { id = facilityId });
        }

        [HttpGet]
        public IActionResult EditRoute(int id)
        {
            var route = context.Routes.FirstOrDefault(r => r.Id == id);
            if (route == null) return BadRequest("Wrong route id");
            ViewData["RouteSteps"] = context.RouteSteps.Where(rs => rs.RouteId == id).ToList();
            ViewData["Beacons"] = context.Beacons.Where(b => b.FacilityId == route.FacilityId).ToList();
            return View(route);
        }

        [HttpPost]
        public IActionResult EditRoute(Route route)
        {
            if (string.IsNullOrEmpty(route.Name) || string.IsNullOrEmpty(route.Content)) ModelState.AddModelError("", "Name or content can not be empty");
            if (!ModelState.IsValid) return View(route);
            context.Routes.Update(route);
            context.SaveChanges();
            return RedirectToAction("Facility", new { id = route.FacilityId});
        }

        [HttpGet]
        public IActionResult AddRouteStep(int routeId, int beaconId, int step)
        {
            var routeStep = new RouteStep
            {
                RouteId = routeId,
                BeaconId = beaconId,
                Step = step
            };
            context.RouteSteps.Add(routeStep);
            context.SaveChanges();
            return RedirectToAction("EditRoute", new { id = routeId});
        }

        [HttpGet]
        public IActionResult DeleteRouteStep(int id, int routeId)
        {
            var routeStep = context.RouteSteps.FirstOrDefault(rs => rs.Id == id);
            if (routeStep == null) return BadRequest($"RouteStep {id} not found.");
            context.RouteSteps.Remove(routeStep);
            context.SaveChanges();
            return RedirectToAction("EditRoute", new { id = routeId});
        }
    }
}