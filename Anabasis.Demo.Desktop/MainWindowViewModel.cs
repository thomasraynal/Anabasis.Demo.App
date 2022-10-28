using Anabasis.Common;
using Anabasis.Demo.Common;
using Anabasis.Demo.Common.Actor;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore.Cache;
using Anabasis.EventStore.Standalone;
using DynamicData;
using DynamicData.Binding;
using EventStore.ClientAPI;
using Lamar;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Serilog;
using Serilog.Events;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace Anabasis.Demo.Desktop
{

    public class DemoSystemRegistry : ServiceRegistry
    {
        public DemoSystemRegistry()
        {
            For<ISerializer>().Use<DefaultSerializer>();
        }
    }

    public class MainWindowViewModel : ReactiveObject
    {
        private readonly TradeActor _tradeSink;
        private readonly MarketDataActor _marketDataSink;

        public ObservableCollectionExtended<TradeViewModel> Trades { get; }
        public ObservableCollectionExtended<MarketDataViewModel> MarketData { get; }

        private TradeViewModel _selectedTrade;
        public TradeViewModel SelectedTrade
        {
            get => _selectedTrade;
            set => this.RaiseAndSetIfChanged(ref _selectedTrade, value);
        }

        private ObservableCollectionExtended<CurrencyPairPositionViewModel> _currencyPairPositions;
        public ObservableCollectionExtended<CurrencyPairPositionViewModel> CurrencyPairPositions
        {
            get => _currencyPairPositions;
            set => this.RaiseAndSetIfChanged(ref _currencyPairPositions, value);
        }

        public MainWindowViewModel()
        {
            RandomlyThrownException.IsEnabled = false;

            Trades = new ObservableCollectionExtended<TradeViewModel>();
            MarketData = new ObservableCollectionExtended<MarketDataViewModel>();
            CurrencyPairPositions = new ObservableCollectionExtended<CurrencyPairPositionViewModel>();

            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
              .WriteTo.Debug()
              .CreateLogger();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog(Log.Logger);

            var eventStoreConnectionString = "ConnectTo=tcp://admin:changeit@localhost:1113; HeartBeatTimeout=1500; VerboseLogging=false; OperationTimeout=60000; UseSslConnection=false;";

            var tradeEventProvdider = new DefaultEventTypeProvider<Trade>(() => new[] {
                    typeof(TradeCreated),
                    typeof(TradeStatusChanged),
                    typeof(TradeValueChanged)
                });

            var marketDataEventHandler = new DefaultEventTypeProvider<MarketData>(() => new[] {
                    typeof(MarketDataChanged)
                });

            var allStreamsCatchupCacheConfiguration = new AllStreamsCatchupCacheConfiguration()
            {
                KeepAppliedEventsOnAggregate = true,
                Checkpoint = Position.Start
            };

            _tradeSink = EventStoreStatefulActorBuilder<TradeActor,AllStreamsCatchupCacheConfiguration, Trade, DemoSystemRegistry>
                                            .Create(eventStoreConnectionString,
                                                    ConnectionSettings.Create(),
                                                    allStreamsCatchupCacheConfiguration,
                                                    ActorConfiguration.Default, 
                                                    tradeEventProvdider)
                                            .Build();

            _marketDataSink = EventStoreStatefulActorBuilder<MarketDataActor,AllStreamsCatchupCacheConfiguration, MarketData, DemoSystemRegistry>
                                            .Create(eventStoreConnectionString,
                                                    ConnectionSettings.Create(),
                                                    allStreamsCatchupCacheConfiguration, 
                                                    ActorConfiguration.Default, 
                                                    marketDataEventHandler)
                                            .Build();

            _marketDataSink.AsObservableCache().Connect()
                                 .ObserveOn(SynchronizationContext.Current)
                                 .Transform(marketData => new MarketDataViewModel(marketData))
                                 .Bind(MarketData)
                                 .Subscribe();

            var tradeObservableCache2 = _tradeSink.AsObservableCache()
                                .Connect()
                                .Sort(SortExpressionComparer<Trade>.Descending(p => p.Timestamp))
                                .ObserveOn(SynchronizationContext.Current)
                                .Transform(trade => new TradeViewModel(trade))
                                .Bind(Trades)
                                .Subscribe();

            var tradeObservableCache = _tradeSink.AsObservableCache()
                                .Connect()
                                .ObserveOn(SynchronizationContext.Current)
                                .Group(trade => trade.CurrencyPair)
                                .Transform(group => new CurrencyPairPositionViewModel(group))
                                .Sort(SortExpressionComparer<CurrencyPairPositionViewModel>.Ascending(t => t.CcyPair))
                                .Bind(CurrencyPairPositions)
                                .Subscribe();
        }
    }
}
