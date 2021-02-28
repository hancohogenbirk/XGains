using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Models.Response.Kraken
{
    public class LedgerResultModel
    {
        public Dictionary<string, LedgerModel> Ledger { get; set; }
    }
}
