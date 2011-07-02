using System;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class BuilderAction
    {
        public BuilderAction( Order order, Action<BuilderContext> action)
        {
            this.Action = action;
            this.Order = order;
        }

        public Order Order { get; private set; }
        public Action<BuilderContext> Action { get; private set; }
        public void Execute( BuilderContext builderContext )
        {
            this.Action( builderContext );
        }
    }
}