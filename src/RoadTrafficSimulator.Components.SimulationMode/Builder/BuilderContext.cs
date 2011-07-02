using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories;
using RoadTrafficSimulator.Infrastructure.Controls;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class BuilderContext
    {
        private readonly Dictionary<IControl, IRoadElement> _elements = new Dictionary<IControl, IRoadElement>();

        public BuilderContext( IConductorFactory conductorFactory )
        {
            this.ConductorFactory = conductorFactory;
        }

        public IEnumerable<IRoadElement> Elements
        {
            get { return _elements.Values.ToArray(); }
        }

        public IConductorFactory ConductorFactory { get; private set; }

        public void AddElement( IControl key, IRoadElement roadElement )
        {
            this._elements.Add( key, roadElement );
        }

        public T GetObject<T>( IControl buildControl ) where T : class
        {
            if ( buildControl == null ) { return null; }

            var result = this._elements[ buildControl ] as T;
            if ( result == null ) { throw new ArgumentException( "Invalid object" ); }
            return result;
        }
    }
}