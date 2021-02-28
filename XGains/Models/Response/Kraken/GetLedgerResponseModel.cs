using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Models.Response.Kraken
{
    public class GetLedgerResponseModel : ResponseModelBase
    {
        public LedgerResultModel Result { get; set; }
    }
}
