using Anabasis.Common;
using ReactiveUI;
using System;

namespace Anabasis.Demo.Desktop
{
    public class TradeEventViewModel : ReactiveObject, IEquatable<TradeEventViewModel>
    {

        private int _position;
        public int Position
        {
            get => _position;
            set => this.RaiseAndSetIfChanged(ref _position, value);
        }

        private string _tradeEventName;
        public string Name
        {
            get => _tradeEventName;
            set => this.RaiseAndSetIfChanged(ref _tradeEventName, value);
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get => _timestamp;
            set => this.RaiseAndSetIfChanged(ref _timestamp, value);
        }

        private string _data;

        public TradeEventViewModel(int position, IEvent @event)
        {
            Name = @event.EventName;
            Position = position;
            Timestamp = @event.Timestamp;
            Data = @event.ToJson();
        }

        public string Data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        public bool Equals(TradeEventViewModel other)
        {
            return Data == other.Data;
        }
    }
}
