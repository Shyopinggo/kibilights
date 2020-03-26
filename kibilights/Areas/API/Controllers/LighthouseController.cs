using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using KibiLights.Models;
using KibiLights.Models.Lighthouse;
using KibiLights.Areas.API.Hubs;

namespace KibiLights.Areas.API.Controllers
{
    [Area("API")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ", " + CookieAuthenticationDefaults.AuthenticationScheme )]
    public class LighthouseController : Controller
    {
        private IHubContext<LighthouseHub> hubContext;
        private ConnectedFacilities connectedFacilities;
        private ApplicationContext dbContext;

        public LighthouseController(IHubContext<LighthouseHub> hubContext, ConnectedFacilities connectedFacilities, ApplicationContext dbContext)
        {
            this.hubContext = hubContext;
            this.connectedFacilities = connectedFacilities;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Call(int facilityId, int id)
        {
            var facility = dbContext.Facilities.Include(f => f.User).FirstOrDefault(f => f.Id == facilityId);
            if (facility == null) return BadRequest($"Facility {facilityId} not found.");
            await hubContext.Clients.User(facility.User.Email).SendAsync("Call", id);
            return Ok();
        }
        
        [HttpGet]
        public JsonResult GetFacilities()
        {
            var facilities = dbContext.Facilities.ToList();
            return Json(facilities);
        }

        [HttpGet]
        public JsonResult GetCities()
        {
            return Json(dbContext.Cities.ToList());
        }

        [HttpGet]
        public JsonResult GetCountries()
        {
            return Json(dbContext.Countries.ToList());
        }

        [HttpGet]
        public IActionResult IsConnected(int id)
        {
            var facility = dbContext.Facilities.Include(f => f.User).FirstOrDefault(f => f.Id == id);
            if (facility == null) return BadRequest($"Facility {id} not found.");
            var result = new
            {
                Id = id,
                Connected = connectedFacilities.IsConnected(facility.User.Email)
        };
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetBeacons(int id)
        {
            return Json(dbContext.Beacons.Where(b => b.FacilityId == id).ToList());
        }

        [HttpGet]
        public JsonResult GetRoutes(int id)
        {
            return Json(dbContext.Routes.Where(r => r.FacilityId == id).ToList());
        }

        [HttpGet]
        public JsonResult GetRouteSteps(int id)
        {
            return Json(dbContext.RouteSteps.Where(rs => rs.RouteId == id).ToList());
        }
    }
}