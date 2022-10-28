using Anabasis.Demo.Common.Actor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.Demo.Api
{
    [ApiController]
    [Route("anabasis-demo-trade-api/v1/ccy")]
    public class MarketDataController : ControllerBase
    {
        private readonly MarketDataSink _marketDataSink;

        public MarketDataController(MarketDataSink marketDataSink)
        {
            _marketDataSink = marketDataSink;
        }

        [HttpGet]
        public IActionResult GetTradedCurrencyPairs()
        {
            return Ok(_marketDataSink.GetCurrents());
        }

        [HttpGet("{id}")]
        public IActionResult GetOneTradedCurrencyPair(string currencyId)
        {
            var currencyPair = _marketDataSink.GetCurrent(currencyId);
            return Ok(currencyPair);
        }

    }
}
