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
    [Route("anabasis-demo-trade-api/v1/trades")]
    public class TradeController : ControllerBase
    {
        private readonly TradeSink _tradeSink;

        public TradeController(TradeSink tradeSink)
        {
            _tradeSink = tradeSink;
        }

        [HttpGet]
        public IActionResult GetTrades()
        {
            return Ok(_tradeSink.State.GetCurrents());
        }

        [HttpGet("{id}")]
        public IActionResult GetOneTrade(string tradeId)
        {
            var trade = _tradeSink.State.GetCurrents().FirstOrDefault(trade => trade.EntityId == tradeId);
            return Ok(trade);
        }

    }
}
