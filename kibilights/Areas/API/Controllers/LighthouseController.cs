using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using KibiLights.Models;
using KibiLights.Models.Lighthouse;
using KibiLights.Areas.API.Hubs;

namespace KibiLights.Areas.API.Controllers
{
    [Area("API")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<IActionResult> Call(string name, int id)
        {
            await hubContext.Clients.User(name).SendAsync("Call", id);
            return Ok();
        }
        
        [HttpGet]
        public JsonResult GetFacilities()
        {
            var facilities = dbContext.Facilities.ToList();
            return Json(facilities);
        }
    }
}