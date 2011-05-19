using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Common.Handler;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common;
using System.Linq;
using CarsInserter = RoadTrafficSimulator.Components.SimulationMode.Elements.CarsInserter;
using CarsRemover = RoadTrafficSimulator.Components.SimulationMode.Elements.CarsRemover;
using CarsInserterBuilder = RoadTrafficSimulator.Components.BuildMode.Controls.CarsInserter;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class Builder
    {
        private readonly CompositeHandler _handlers = new CompositeHandler();
        private readonly Dictionary<object, IRoadElement> _elements = new Dictionary<object, IRoadElement>();
        private readonly List<Action> _connectElementsAction = new List<Action>();
        private readonly IEventAggregator _eventAggregator;

        public Builder( IEventAggregator eventAggregator )
        {
            Contract.Requires( eventAggregator != null );
            this._eventAggregator = eventAggregator;
            this._handlers.Register<BuildMode.Controls.CarsInserter>( this.OnCarsInserter );
            this._handlers.Register<BuildMode.Controls.CarsRemover>( this.OnCarsRemover );
            this._handlers.Register<RoadJunctionBlock>( this.OnLaneJuntion );
            this._handlers.Register<RoadConnection>( this.OnLaneCorner );
        }

        public IEnumerable<IRoadElement> ConvertToSimulationMode( IEnumerable<IControl> controls )
        {
            controls.ForEach( s => this._handlers.Handle( s ) );
            this._connectElementsAction.ForEach( a => a() );
            return this._elements.Values.ToArray();
        }

        private void OnLaneCorner( RoadConnection roadConnection )
        {
            var laneCorner = new LaneCorner( roadConnection );
            this._elements.Add( roadConnection, laneCorner );
            this._connectElementsAction.Add( () => this.ConectLaneCorner( laneCorner ) );
        }

        private void ConectLaneCorner( LaneCorner laneCorner )
        {
            laneCorner.Prev = this.GetObject<Lane>( laneCorner.Prev );
            laneCorner.Next = this.GetObject<Lane>( laneCorner.Next );
        }

        private void OnLaneJuntion( RoadJunctionBlock roadJunctionBlock )
        {
            var element = new LaneJunction( roadJunctionBlock );
            this._elements.Add( roadJunctionBlock, element );
            this._connectElementsAction.Add( () => this.ConnectLaneJucntion( element ) );
        }

        private void ConnectLaneJucntion( LaneJunction element )
        {
            element.Bottom.Lane = this.GetObject<Lane>( element.JunctionBuilder.RoadJunctionEdges[ EdgeType.Bottom ].Connector.Edge );
            element.Left.Lane = this.GetObject<Lane>( element.JunctionBuilder.RoadJunctionEdges[ EdgeType.Left ].Connector.Edge );
            element.Top.Lane = this.GetObject<Lane>( element.JunctionBuilder.RoadJunctionEdges[ EdgeType.Top ].Connector.Edge );
            element.Right.Lane = this.GetObject<Lane>( element.JunctionBuilder.RoadJunctionEdges[ EdgeType.Right ].Connector.Edge );
        }

        private void OnCarsRemover( BuildMode.Controls.CarsRemover carsRemover )
        {
            var element = new CarsRemover( carsRemover, c => new CarRemoverConductor( c, this._eventAggregator ) );
            this._elements.Add( carsRemover, element );
            this._connectElementsAction.Add( () => this.ConnectCarsRemover( element ) );
        }

        private void ConnectCarsRemover( CarsRemover element )
        {
            element.Lane = this.GetObject<Lane>( element.CarsRemoverBuilder.Connector.ConnectedRoad );
        }

        private void OnCarsInserter( BuildMode.Controls.CarsInserter carsInserter )
        {
            var element = new CarsInserter( carsInserter, c => new CarInserterConductor( c ) );
            this._elements.Add( carsInserter, element );
            this._connectElementsAction.Add( () => this.ConnectCarsInserter( element ) );
        }

        private void ConnectCarsInserter( CarsInserter element )
        {
            element.Lane = this.GetObject<Lane>( element.CarsInserterBuilder.Connector.ConnectedRoad );
        }

        private T GetObject<T>( object @object ) where T : class
        {
            if ( @object == null ) { return null; }

            var result = this._elements[ @object ] as T;
            if ( result == null ) { throw new ArgumentException( "Invalid object" ); }
            return result;
        }
    }
}