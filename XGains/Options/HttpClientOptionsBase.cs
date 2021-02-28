using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Options
{
    public abstract class HttpClientOptionsBase
    {
        public string BaseUrl { get; set; }

        public virtual Dictionary<string, string> DefaultHeaders { get; } =
            new Dictionary<string, string>();
    }
}
