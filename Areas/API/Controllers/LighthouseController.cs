using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KibiLights.Areas.API.Controllers
{
    [Area("API")]
    public class LighthouseController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello, world!");
        }
    }
}