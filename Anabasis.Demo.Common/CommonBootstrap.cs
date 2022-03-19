using Anabasis.Api;
using Anabasis.EventStore;
using Anabasis.EventStore.Connection;
using Anabasis.EventStore.Snapshot;
using Anabasis.RabbitMQ;
using EventStore.ClientAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anabasis.Demo.Common
{
    public static class Common
    {
        public static (DemoConfigurationOptions demoConfigurationOptions, ConnectionSettingsBuilder connectionSettingsBuilder, EventStoreConnectionOptions eventStoreConnectionOptions) Bootstrap(this IServiceCollection serviceCollection, IConfigurationRoot configurationRoot)
        {

            var connectionSettingsBuilder = ConnectionSettings
                    .Create()
                    .DisableTls()
                    .DisableServerCertificateValidation()
                    .EnableVerboseLogging()
                    .UseDebugLogger()
                    .KeepReconnecting()
                    .KeepRetrying()
                    .SetDefaultUserCredentials(StaticData.UserCredentials);

            serviceCollection.WithConfiguration<RabbitMqConnectionOptions>(configurationRoot);

            var eventStoreConnectionOptions = serviceCollection.WithConfiguration<EventStoreConnectionOptions>(configurationRoot);
            var demoConfigurationOptions = serviceCollection.WithConfiguration<DemoConfigurationOptions>(configurationRoot);

            RandomlyThrownException.IsEnabled = demoConfigurationOptions.ShouldRandomlyThrow;

            serviceCollection.AddSingleton<IEventStoreBus, EventStoreBus>();
            serviceCollection.AddSingleton<IRabbitMqBus, RabbitMqBus>();
            serviceCollection.AddSingleton<ISnapshotStrategy, DefaultSnapshotStrategy>();

            return (demoConfigurationOptions, connectionSettingsBuilder, eventStoreConnectionOptions);

        }
    }
}
