using System;
using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class EmptyConnectionSupport<TOwner> : IConnectionSupport where TOwner : IControl
    {
        private readonly TOwner _owner;

        public EmptyConnectionSupport( TOwner owner )
        {
            this._owner = owner;
        }

        public IControl Owner
        {
            get { return this._owner; }
        }

        public IConnectionSupport Left
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        public IConnectionSupport Top
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        public IConnectionSupport Right
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        public IConnectionSupport Bottom
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        public IEnumerable<IConnectionSupport> ConnectedObject
        {
            get { return Enumerable.Empty<IConnectionSupport>(); }
        }

        public void Connect( IConnectionSupport objectToConnect )
        {
        }
    }
}