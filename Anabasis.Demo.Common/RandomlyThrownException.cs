using System;

namespace Anabasis.Demo.Common
{
    public static class RandomlyThrownException
    {
        private static readonly Random _rand = new(Guid.NewGuid().GetHashCode());
        public static bool IsEnabled { get; set; } = true;

        public static void MaybeThrowSomething(double averageOccurenceInPercent = 0.1)
        {
            if (!IsEnabled) return;

            var next = _rand.NextDouble();

            if (next <= averageOccurenceInPercent)
                throw new Exception("Fate has chosen you!");
        }

    }
}
