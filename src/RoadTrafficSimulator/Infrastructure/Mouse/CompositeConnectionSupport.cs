using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class CompositeConnectionSupport<TOwner> : IConnectionCompositeSupport where TOwner : IControl
    {
        private TOwner _owner;

        public CompositeConnectionSupport( TOwner owner)
        {
            this._owner = owner;
        }

        public IControl Owner
        {
            get { return this._owner; }
        }

        public IEnumerable<IConnectionSupport> ConnectedObject
        {
            get { throw new NotImplementedException(); }
        }

        public void Connect( IConnectionSupport objectToConnect )
        {
            throw new NotImplementedException();
        }

        public void ConnectChildren( IControl children, IConnectionSupport objectToConnect )
        {
            throw new NotImplementedException();
        }
    }
}