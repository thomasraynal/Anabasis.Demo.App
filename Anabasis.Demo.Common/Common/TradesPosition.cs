using System;

namespace Anabasis.Demo
{
    public class TradesPosition
    {

        public TradesPosition(double buy, double sell, int count)
        {
            Buy = buy;
            Sell = sell;
            Count = count;
            Position = Buy - Sell;
        }

        public bool Negative => Position < 0;

        public double Position { get; }
        public double Buy { get; }
        public double Sell { get; }
        public int Count { get; }
    }
}
