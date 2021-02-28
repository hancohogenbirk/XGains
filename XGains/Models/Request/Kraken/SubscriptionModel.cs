using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XGains.Models.Request.Kraken
{
    public class SubscriptionModel
    {
        [JsonIgnore]
        public int Interval { get; set; }
        public string Name { get; set; }
    }
}
