using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using KibiLights.Models;

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
    }
}