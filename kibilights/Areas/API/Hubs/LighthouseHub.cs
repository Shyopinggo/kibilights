using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace KibiLights.Areas.API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LighthouseHub : Hub
    {
        public async Task Test(int id)
        {
            await Clients.Caller.SendAsync("Call", id);
        }
    }
}
