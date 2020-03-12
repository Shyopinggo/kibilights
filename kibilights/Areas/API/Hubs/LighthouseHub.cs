using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace KibiLights.Areas.API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Spark")]
    public class LighthouseHub : Hub
    {
        private ConnectedFacilities connections;

        public LighthouseHub(ConnectedFacilities connections)
        {
            this.connections = connections;
        }
        
        public async Task Test(int id)
        {
            await Clients.Caller.SendAsync("Call", id);
        }

        public override Task OnConnectedAsync()
        {
            connections.Add(Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            connections.Remove(Context.User.Identity.Name);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
