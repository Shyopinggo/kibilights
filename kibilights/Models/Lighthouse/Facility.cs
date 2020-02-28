using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KibiLights.Models.Lighthouse
{
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public List<Beacon> Beacons { get; set; }
        public List<Route> Routes { get; set; }
    }
}
