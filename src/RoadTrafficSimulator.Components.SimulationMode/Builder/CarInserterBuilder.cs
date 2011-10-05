using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class CarInserterBuilder : IBuilerItem
    {
        public IEnumerable<BuilderAction> Create( IControl control )
        {
            var builder = new Builder();
            yield return new BuilderAction( Order.High, context => builder.Build( context, control ) );
            yield return new BuilderAction( Order.Normal, builder.Connect );
            yield return new BuilderAction( Order.Low, builder.SetUp );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( BuildMode.Controls.CarsInserter );
        }

        private class Builder
        {
            private CarsInserter _carsInserter;

            public void Build( BuilderContext context, IControl control )
            {
                var roadJunctionBlock = ( BuildMode.Controls.CarsInserter ) control;
                this._carsInserter = new CarsInserter( roadJunctionBlock, c => context.RoadInformationFactory.Create( c ) );
                context.AddElement( roadJunctionBlock, this._carsInserter );
            }

            public void Connect( BuilderContext builderContext )
            {
                var connectedLane = this._carsInserter.CarsInserterBuilder.Connector.OpositeEdge;
                if ( connectedLane == null ) { return; }
                this._carsInserter.Lane = builderContext.GetObject<Lane>( connectedLane.Parent );
            }

            public void SetUp( BuilderContext obj )
            {
            }
        }
    }

    public class Routes
    {
        private readonly static Routes _empty = new Routes( Enumerable.Empty<Route>() );

        public static Routes Empty
        {
            get { return _empty; }
        }

        private List<Route> _routes;

        public Routes( IEnumerable<Route> routes )
        {
            this._routes = new List<Route>( routes );
        }

        public IEnumerable<IRoadElement> GetRandomRoute( Random rng )
        {
            var rngNumber = rng.Next( 0, 100 );
            var previous = 0.0;
            for ( var i = 0; i < this._routes.Count; i++ )
            {
                if ( previous > rngNumber && rngNumber < this._routes[ i ].Probability )
                {
                    return this._routes[ i ].Elements;
                }
                previous = this._routes[ i ].Probability;
            }

            return Enumerable.Empty<IRoadElement>();
        }

        public void CalculateProbabilities()
        {
            var alreadySet = this._routes.Where( s => s.Probability != 0 ).ToArray();
            var inUse = alreadySet.Sum( s => s.Probability );

            var left = this._routes.Count - alreadySet.Length;
            if ( left == 0 ) { return; }

            var perItem = ( 100 - inUse ) / left;
            this._routes.Where( s => s.Probability == 0 ).ForEach( s => s.Probability = perItem );
        }
    }

    public class Route
    {
        private readonly List<IRoadElement> _elements;

        public Route( IEnumerable<IRoadElement> elements )
        {
            this._elements = new List<IRoadElement>( elements );
        }

        public IEnumerable<IRoadElement> Elements { get { return this._elements; } }

        public float Probability { get; set; }
    }
}