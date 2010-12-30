using System;
using System.Collections.Generic;
using Common;
using RoadTrafficSimulator.Infrastructure.Control;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.Road.RoadJoiners;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class RoadJunctionConnectionSupport : IConnectionCompositeSupport
    {
        private readonly IRoadJunctionBlock _owner;
        private readonly IList<IConnectionSupport> _connectedObject = new List<IConnectionSupport>();
        private readonly IConnectionSupport[] _connectedSides = new IConnectionSupport[EdgeType.Count];

        public RoadJunctionConnectionSupport( IRoadJunctionBlock owner )
        {
            this._owner = owner.NotNull();
        }

        public IControl Owner
        {
            get { return this._owner; }
        }

        public IEnumerable<IConnectionSupport> ConnectedObject
        {
            get { return this._connectedObject; }
        }

        public void Connect( IConnectionSupport objectToConnect )
        {
            this._connectedObject.Add( objectToConnect );
        }

        public void ConnectChildren( IControl children, IConnectionSupport objectToConnect )
        {
            var index = Array.IndexOf( this._owner.RoadJunctionEdges, children );
            if ( index == -1 )
            {
                throw new ArgumentException();
            }

            this.EnsureSlotIsFree( index );
            this._connectedSides[ index ] = objectToConnect;
        }

        private void EnsureSlotIsFree( int index )
        {
            if ( index >= this._connectedSides.Length )
            {
                throw new ArgumentException();
            }

            if ( this._connectedSides[ index ] != null )
            {
                throw new ArgumentException();
            }
        }
    }
}