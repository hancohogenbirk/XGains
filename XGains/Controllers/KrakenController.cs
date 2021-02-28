using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XGains.Clients;
using XGains.Models.Response.Kraken;

namespace XGains.Controllers
{
    public class KrakenController : ControllerBase
    {
        private readonly IKrakenClient _krakenClient;

        public KrakenController(IKrakenClient krakenClient)
        {
            _krakenClient = krakenClient;
        }

        [HttpGet("ledger")]
        public async Task<GetLedgerResponseModel> GetLedger()
        {
            var parameters = new Dictionary<string, string>
            {
                { "asset", "ZEUR" },
                { "type", "deposit" }
            };

            var result = await _krakenClient.PostUrlEncoded<GetLedgerResponseModel>("/0/private/Ledgers", parameters);
            return result;
        }
    }
}
