using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Options
{
    public class KrakenClientOptions : HttpClientOptionsBase
    {
        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }

        public override Dictionary<string, string> DefaultHeaders { get; } =
            new Dictionary<string, string>
            {
                {"user-agent", "insomnia/2020.5.2"},
                {"accept", "*/*"}
            };
    }
}
