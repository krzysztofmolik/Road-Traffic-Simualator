using System;
using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Control;
using XnaRoadTrafficConstructor.Road;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class ConnectionSupport<TOwner> : IConnectionSupport where TOwner : IControl
    {
        private readonly IConnectionSupport[] _connected = new IConnectionSupport[ EdgeType.Count ];

        private readonly TOwner _owner;

        public ConnectionSupport( TOwner owner )
        {
            this._owner = owner;
        }

        public IControl Owner { get { return this._owner; } }

        public IEnumerable<IConnectionSupport> ConnectedObject
        {
            get { return this._connected.Where( c => c != null ).ToArray(); }
        }

        public void Connect( IConnectionSupport objectToConnect )
        {
            throw new NotImplementedException();
        }
    }
}