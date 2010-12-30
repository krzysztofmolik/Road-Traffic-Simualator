using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class RoadLaneConnectionSupport : IConnectionSupport 
    {
        private readonly IConnectionSupport _firstSlot;
        private readonly IConnectionSupport _secondSlot;

        private readonly RoadLaneConnection _owner;

        public RoadLaneConnectionSupport( RoadLaneConnection owner )
        {
            this._firstSlot = null;
            this._secondSlot = null;
            this._owner = owner;
        }

        public IControl Owner { get { return this._owner; } }

        public IEnumerable<IConnectionSupport> ConnectedObject
        {
            get { return new[] { this._firstSlot, this._secondSlot }; }
        }

        public void Connect( IConnectionSupport objectToConnect )
        {
        }
    }
}