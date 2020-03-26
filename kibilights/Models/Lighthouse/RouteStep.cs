using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KibiLights.Models.Lighthouse
{
    public class RouteStep
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
        public int? BeaconId { get; set; }
        public int Step { get; set; }
    }
}
