using System;
using System.Collections.Generic;
using Common.Handler;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Draw;
using XnaRoadTrafficConstructor.Road;
using CarsInserterBuild = RoadTrafficSimulator.Road.Controls.CarsInserter;
using CarsRemoverBuild = RoadTrafficSimulator.Road.Controls.CarsRemover;
using LaneJunctionBuilder = RoadTrafficSimulator.Road.Controls.RoadJunctionBlock;
using LaneCornerBuilder = RoadTrafficSimulator.Road.Controls.RoadConnection;
using Common;

namespace RoadTrafficSimulator.RoadComponents
{
    public class Builder
    {
        private readonly CompositeHandler _handlers = new CompositeHandler();
        private readonly Dictionary<object, IRoadElement> _elements = new Dictionary<object, IRoadElement>();
        private readonly List<Action> _connectElementsAction = new List<Action>();

        public Builder()
        {
            this._handlers.Register<CarsInserterBuild>( this.OnCarsInserter );
            this._handlers.Register<CarsRemoverBuild>( this.OnCarsRemover );
            this._handlers.Register<LaneJunctionBuilder>( this.OnLaneJuntion );
            this._handlers.Register<LaneCornerBuilder>( this.OnLaneCorner );
        }

        public TrafficModel Build( IEnumerable<IControl> controls )
        {
            controls.ForEach( s => this._handlers.Handle( s ) );
            this._connectElementsAction.ForEach( a => a() );

            var result = new TrafficModel();
        }

        private void OnLaneCorner( LaneCornerBuilder roadConnection )
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

        private void OnLaneJuntion( LaneJunctionBuilder roadJunctionBlock )
        {
            var element = new LaneJunction( roadJunctionBlock );
            this._elements.Add( roadJunctionBlock, element );
            this._connectElementsAction.Add( () => this.ConnectLaneJucntion( element ) );
        }

        private void ConnectLaneJucntion( LaneJunction element )
        {
            var bottom = new JunctionEdge();
            bottom.Junction = element;
            bottom.Lane = this.GetObject<Lane>( element.JunctionBuilder.RoadJunctionEdges[ EdgeType.Bottom ].Connector.Edge );

            var left = new JunctionEdge();
            left.Junction = element;
            left.Lane = this.GetObject<Lane>( element.JunctionBuilder.RoadJunctionEdges[ EdgeType.Left ].Connector.Edge );

            var top = new JunctionEdge();
            top.Junction = element;
            top.Lane = this.GetObject<Lane>( element.JunctionBuilder.RoadJunctionEdges[ EdgeType.Top ].Connector.Edge );

            var right = new JunctionEdge();
            right.Junction = element;
            right.Lane = this.GetObject<Lane>( element.JunctionBuilder.RoadJunctionEdges[ EdgeType.Right ].Connector.Edge );

            element.Bottom = bottom;
            element.Left = left;
            element.Top = top;
            element.Right = right;
        }

        private void OnCarsRemover( CarsRemoverBuild carsRemover )
        {
            var element = new CarsRemover( carsRemover );
            this._elements.Add( carsRemover, element );
            this._connectElementsAction.Add( () => this.ConnectCarsRemover( element ) );
        }

        private void ConnectCarsRemover( CarsRemover element )
        {
            element.Lane = this.GetObject<Lane>(element.CarsRemoverBuilder.Connector.ConnectedRoad);
        }

        private void OnCarsInserter( CarsInserterBuild carsInserter )
        {
            var element = new CarsInserter( carsInserter );
            this._elements.Add( carsInserter, element );
            this._connectElementsAction.Add( () => this.ConnectCarsInserter( element ) );
        }

        private void ConnectCarsInserter(CarsInserter element)
        {
            element.Lane = this.GetObject<Lane>(element.CarsInserterBuilder.Connector.ConnectedRoad);
        }

        private T GetObject<T>( object @object ) where T : class
        {
            if ( @object == null ) { return null; }

            var result = this._elements[ @object ] as T;
            if ( result == null ) { throw new ArgumentException( "Invalid object" ); }
            return result;
        }
    }

    public class CarsRemover : IRoadElement
    {
        public CarsRemover( CarsRemoverBuild vertexContainer )
        {
            this.CarsRemoverBuilder = vertexContainer;
        }

        public CarsRemoverBuild CarsRemoverBuilder { get; private set; }

        public Lane Lane { get; set; }
    }

    public class Lane : IRoadElement
    {
        public LaneConnector Prev { get; set; }
        public LaneConnector Next { get; set; }
        public Lane Top { get; set; }
        public Lane Bottom { get; set; }
    }

    public class CarsInserter : LaneConnector, IRoadElement
    {
        private readonly IVertexContainer _vertexContainer;

        public CarsInserter( CarsInserterBuild carsInserterBuilder )
        {
            this.CarsInserterBuilder = carsInserterBuilder;
        }

        public CarsInserterBuild CarsInserterBuilder { get; private set; }

        public Lane Lane { get; set; }
    }

    public class JunctionEdge : IRoadElement
    {
        public LaneJunction Junction { get; set; }
        public Lane Lane { get; set; }
    }

    public class LaneJunction : IRoadElement
    {
        public LaneJunction( LaneJunctionBuilder junctionBuilder )
        {
            this.JunctionBuilder = junctionBuilder;
        }

        public LaneJunctionBuilder JunctionBuilder { get; private set; }
        public JunctionEdge Left { get; set; }
        public JunctionEdge Top { get; set; }
        public JunctionEdge Right { get; set; }
        public JunctionEdge Bottom { get; set; }
    }

    public class LaneCorner : LaneConnector, IRoadElement
    {
        public LaneCorner( LaneCornerBuilder vertexContainer )
        {
            this.LaneCornerBuild = vertexContainer;
        }

        public LaneCornerBuilder LaneCornerBuild { get; private set; }

        public Lane Prev { get; set; }
        public Lane Next { get; set; }
    }

    public abstract class LaneConnector : IRoadElement
    {
    }
}