using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Models.Response.Kraken
{
    public abstract class ResponseModelBase
    {
        public List<string> Error { get; set; }
    }
}
