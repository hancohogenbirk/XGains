using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Models.Request.Kraken
{
    public class SubscribeMarketPairModel
    {
        public string Event { get; set; }

        public IEnumerable<string> Pair { get; set; }

        public SubscriptionModel Subscription { get; set; }
    }
}
