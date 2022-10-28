using Anabasis.Common;
using Anabasis.Common.Configuration;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using DynamicData;
using DynamicData.Kernel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Actor
{
    public class TradeGenerator : BaseStatelessActor
    {
        private readonly Random _random = new();
        private readonly IDictionary<string, MarketData> _latestPrices = new Dictionary<string, MarketData>();
        private readonly object _locker = new();
        private SourceCache<Trade, string> _sourceCache;

        public TradeGenerator(IActorConfigurationFactory actorConfigurationFactory, ILoggerFactory loggerFactory = null) : base(actorConfigurationFactory, loggerFactory)
        {
            Initialize();
        }

        public TradeGenerator(IActorConfiguration actorConfiguration, ILoggerFactory loggerFactory = null) : base(actorConfiguration, loggerFactory)
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var ccyPair in StaticData.CurrencyPairs)
            {
                _latestPrices[ccyPair.EntityId] = new MarketData(ccyPair.EntityId, ccyPair.InitialPrice, ccyPair.InitialPrice);
            }
        }

        public override async Task OnInitialized()
        {
     
            _sourceCache = new SourceCache<Trade, string>((trade) => trade.EntityId);

            await Task.Delay(3000);

            await GenerateTradesAndMaintainCache();

        }

        public Task Handle(MarketDataQuoteChanged marketDataChanged)
        {
            _latestPrices[marketDataChanged.EntityId] = new MarketData(marketDataChanged.EntityId, marketDataChanged.Bid, marketDataChanged.Offer);
            return Task.CompletedTask;
        }

        public IEnumerable<Trade> Generate(int numberToGenerate, bool initialLoad = false)
        {
            Trade NewTrade()
            {
                var id = $"{Guid.NewGuid()}";
                var bank = StaticData.Customers[_random.Next(0, StaticData.Customers.Length)];
                var pair = StaticData.CurrencyPairs[_random.Next(0, StaticData.CurrencyPairs.Length)];
                var amount = (_random.Next(1, 2000) / 2) * (10 ^ _random.Next(1, 5));
                var buySell = _random.Next(0, 2) == 1 ? BuyOrSell.Buy : BuyOrSell.Sell;
                var desk = StaticData.Desks[_random.Next(0, StaticData.Desks.Length)];

                if (initialLoad)
                {
                    var status = _random.NextDouble() > 0.5 ? TradeStatus.Live : TradeStatus.Closed;
                    var seconds = _random.Next(1, 60 * 60 * 24);
                    var time = DateTime.Now.AddSeconds(-seconds);
                    return new Trade(id, bank, desk, pair.EntityId, status, buySell, GenerateRandomPrice(pair, buySell), amount, timeStamp: time);
                }

                return new Trade(id, bank, desk, pair.EntityId, TradeStatus.Live, buySell, GenerateRandomPrice(pair, buySell), amount);
            }


            IEnumerable<Trade> result;

            lock (_locker)
            {
                result = Enumerable.Range(1, numberToGenerate).Select(_ => NewTrade()).ToArray();
            }
            return result;
        }

        private double GenerateRandomPrice(CurrencyPair currencyPair, BuyOrSell buyOrSell)
        {

            var price = _latestPrices.Lookup(currencyPair.EntityId)
                                     .ConvertOr(md => md.Bid, () => currencyPair.InitialPrice);

            var pipsFromMarket = _random.Next(1, 100);
            var adjustment = Math.Round(pipsFromMarket * currencyPair.PipSize, currencyPair.DecimalPlaces);
            return buyOrSell == BuyOrSell.Sell ? price + adjustment : price - adjustment;
        }

        private async Task GenerateTradesAndMaintainCache()
        {
            var random = new Random();

            var initialTrades = Generate(2, true);

            _sourceCache.AddOrUpdate(initialTrades);

            foreach (var trade in initialTrades)
            {

                var createTradeCommand = new CreateTrade(
                    trade.EntityId,
                    trade.Counterparty,
                    trade.CurrencyPair,
                    trade.Desk,
                    trade.BuyOrSell,
                    trade.MarketPrice,
                    trade.TradePrice,
                    trade.Amount,
                    Guid.NewGuid());

                Logger.LogInformation($"{createTradeCommand}");

                await this.EmitEventStore(createTradeCommand);
            }

            TimeSpan getRandomInterval() => TimeSpan.FromMilliseconds(random.Next(2500, 5000));

            var tradeGenerator = TaskPoolScheduler.Default
                .ScheduleRecurringAction(getRandomInterval, async () =>
                {
                try
                {
                    var number = random.Next(1, 5);
                    var trades = Generate(number);

                    _sourceCache.AddOrUpdate(trades);

                    foreach (var trade in trades)
                    {
                        var createTradeCommand = new CreateTrade(
                            trade.EntityId,
                            trade.Counterparty,
                            trade.CurrencyPair,
                            trade.Desk,
                            trade.BuyOrSell,
                            trade.MarketPrice,
                            trade.TradePrice,
                            trade.Amount,
                            Guid.NewGuid());

                        Logger.LogInformation($"{createTradeCommand}");

                        await this.EmitEventStore(createTradeCommand);
                    }
                    }
                    catch (Exception ex)
                    {

                    }
                });

            var tradeCloser = TaskPoolScheduler.Default
                .ScheduleRecurringAction(getRandomInterval, () =>
                {
                    try
                    {

                        var number = random.Next(1, 2);

                        _sourceCache.Edit(innerCache =>
                        {
                            var trades = innerCache.Items
                                .Where(trade => trade.Status == TradeStatus.Live)
                                .OrderBy(t => Guid.NewGuid()).Take(number).ToArray();

                            var toClose = trades.Select(trade => new Trade(trade, TradeStatus.Closed));

                            _sourceCache.AddOrUpdate(toClose);

                            foreach (var trade in toClose)
                            {
                                var changeTradeStatus = new ChangeTradeStatus(trade.EntityId, trade.Status, Guid.NewGuid());

                                Logger.LogInformation($"{changeTradeStatus}");

                                this.EmitEventStore(changeTradeStatus).Wait();
                            }
                        });

                    }
                    catch(Exception ex)
                    {

                    }
                });


            AddToCleanup(tradeCloser);
            AddToCleanup(tradeGenerator);

        }
    }
}
