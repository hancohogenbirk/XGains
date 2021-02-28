using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Hubs
{
    public class BalanceHub : Hub<IBalanceHub>
    {
        public async Task BroadcastData(string rawData)
        {
            await Clients.All.BroadcastData(rawData);
        }
    }
}
