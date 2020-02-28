using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KibiLights.Models.Lighthouse
{
    public class Beacon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FacilityId { get; set; }
        public Facility Facility { get; set; }
    }
}
