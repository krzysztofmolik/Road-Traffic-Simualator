
using System;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    public abstract class SingleControl<TVertex> : ControlBaseBase<TVertex>, ISingleControl
    {
        protected SingleControl(IControl parent) : base(parent)
        {
        }

        public abstract IConnectionSupport ConnectionSupport { get; }
    }
}