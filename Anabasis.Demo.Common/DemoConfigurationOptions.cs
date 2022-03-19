using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common
{
    public class DemoConfigurationOptions
    {
        public bool ShouldRandomlyThrow { get; set; }
        public long MarketDataFrequencyIntervalInMilliseconds { get; set; } = 1000;
    }
}
