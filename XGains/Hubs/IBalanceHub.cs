using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Hubs
{
    public interface IBalanceHub
    {
        Task BroadcastData(string rawData);
    }
}
