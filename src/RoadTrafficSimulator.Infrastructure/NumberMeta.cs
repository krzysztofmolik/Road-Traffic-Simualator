using System.Collections.Generic;

namespace RoadTrafficSimulator.Infrastructure
{
    public class NumberMeta : MetaBase
    {
        public NumberMeta( IDictionary<string, object> properties )
            : base( properties )
        {
        }

        public int Order { get; set; }
    }
}