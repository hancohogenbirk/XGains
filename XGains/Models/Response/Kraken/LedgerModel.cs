using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Models.Response.Kraken
{
    public class LedgerModel
    {
        public string RefId { get; set; }

        public decimal Time { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public string AClass { get; set; }

        public string Asset { get; set; }

        public string Amount { get; set; }

        public string Fee { get; set; }

        public string Balance { get; set; }
    }
}
