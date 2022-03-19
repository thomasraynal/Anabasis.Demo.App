using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.Demo
{
    public class CcyPairReporting : Dictionary<string, (double bid, double offer, double spread, bool IsUp)>
    {
        public static CcyPairReporting Empty = new();

        public CcyPairReporting(CcyPairReporting previous)
        {
            foreach (var keyValue in previous)
            {
                this[keyValue.Key] = keyValue.Value;
            }
        }

        public CcyPairReporting()
        {
        }
    }
}
