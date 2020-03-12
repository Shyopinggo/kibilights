using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KibiLights.Areas.API.Hubs
{
    public class ConnectedFacilities
    {
        private HashSet<string> connections;

        public ConnectedFacilities()
        {
            connections = new HashSet<string>();
        }

        public bool Add(string name)
        {
            return connections.Add(name);
        }

        public bool IsConnected(string name)
        {
            return connections.Contains(name);
        }

        public bool Remove(string name)
        {
            return connections.Remove(name);
        }
    }
}
