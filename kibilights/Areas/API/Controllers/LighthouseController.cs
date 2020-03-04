using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using KibiLights.Areas.API.Hubs;

namespace KibiLights.Areas.API.Controllers
{
    [Area("API")]
    [Authorize]
    public class LighthouseController : Controller
    {
        private IHubContext<LighthouseHub> hubContext;

        public LighthouseController(IHubContext<LighthouseHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Call(string name, int id)
        {
            await hubContext.Clients.User(name).SendAsync("Call", id);
            return Ok();
        }
        
        public IActionResult Index()
        {
            return Content("Hello, world!");
        }
    }
}