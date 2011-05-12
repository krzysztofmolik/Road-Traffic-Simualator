using System;
using System.Collections.Generic;
using Common.Handler;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.Creators;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using Common;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class Builder
    {
        private readonly CompositeHandler _handlers = new CompositeHandler();
        private readonly Dictionary<object, IRoadElement> _elements = new Dictionary<object, IRoadElement>();
        private readonly List<Action> _connectElementsAction = new List<Action>();

        public Builder()
        {
            this._handlers.Register<BuildMode.Controls.CarsInserter>( this.OnCarsInserter );
            this._handlers.Register<BuildMode.Controls.CarsRemover>( this.OnCarsRemover );
            this._handlers.Register<RoadJunctionBlock>( this.OnLaneJuntion );
            this._handlers.Register<RoadConnection>( this.OnLaneCorner );
        }

        public SimulationModeMainComponent Build( IEnumerable<IControl> controls )
        {
            controls.ForEach( s => this._handlers.Handle( s ) );
            this._connectElementsAction.ForEach( a => a() );

            // TODO Implement
            return null;
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
            var element = new CarsRemover( carsRemover );
            this._elements.Add( carsRemover, element );
            this._connectElementsAction.Add( () => this.ConnectCarsRemover( element ) );
        }

        private void ConnectCarsRemover( CarsRemover element )
        {
            element.Lane = this.GetObject<Lane>( element.CarsRemoverBuilder.Connector.ConnectedRoad );
        }

        private void OnCarsInserter( BuildMode.Controls.CarsInserter carsInserter )
        {
            var element = new CarsInserter( carsInserter );
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

    public class CarsRemover : RoadElementBase
    {
        public CarsRemover( BuildMode.Controls.CarsRemover control )
            : base( control )
        {
            this.CarsRemoverBuilder = control;
        }

        public BuildMode.Controls.CarsRemover CarsRemoverBuilder { get; private set; }

        public Lane Lane { get; set; }
    }

    public class Lane : RoadElementBase
    {
        public Lane( RoadLaneBlock lane )
            : base( lane )
        {
            this.RoadLaneBlock = lane;
        }

        protected RoadLaneBlock RoadLaneBlock { get; set; }

        public LaneConnector Prev { get; set; }
        public LaneConnector Next { get; set; }
        public Lane Top { get; set; }
        public Lane Bottom { get; set; }
    }

    public class CarsInserter : LaneConnector
    {
        private readonly IVertexContainer _vertexContainer;

        public CarsInserter( BuildMode.Controls.CarsInserter control )
            : base( control )
        {
            this.CarsInserterBuilder = control;
        }

        public BuildMode.Controls.CarsInserter CarsInserterBuilder { get; private set; }

        public Lane Lane { get; set; }
    }

    public class JunctionEdge : RoadElementBase
    {
        public JunctionEdge( RoadJunctionEdge edge )
            : base( edge )
        {
            this.EdgeBuilder = edge;
        }

        protected RoadJunctionEdge EdgeBuilder { get; set; }

        public LaneJunction Junction { get; set; }
        public Lane Lane { get; set; }
    }

    public class LaneJunction : RoadElementBase
    {
        public LaneJunction( RoadJunctionBlock control )
            : base( control )
        {
            this.JunctionBuilder = control;
            this.Left = new JunctionEdge( control.RoadJunctionEdges[ EdgeType.Left ] );
            this.Top = new JunctionEdge( control.RoadJunctionEdges[ EdgeType.Top ] );
            this.Right = new JunctionEdge( control.RoadJunctionEdges[ EdgeType.Right ] );
            this.Bottom = new JunctionEdge( control.RoadJunctionEdges[ EdgeType.Bottom ] );

            this.Left.Junction = this;
            this.Top.Junction = this;
            this.Right.Junction = this;
            this.Bottom.Junction = this;
        }

        public RoadJunctionBlock JunctionBuilder { get; private set; }
        public JunctionEdge Left { get; set; }
        public JunctionEdge Top { get; set; }
        public JunctionEdge Right { get; set; }
        public JunctionEdge Bottom { get; set; }
    }

    public class LaneCorner : LaneConnector
    {
        public LaneCorner( RoadConnection control )
            : base( control )
        {
            this.LaneCornerBuild = control;
        }

        public RoadConnection LaneCornerBuild { get; private set; }

        public Lane Prev { get; set; }
        public Lane Next { get; set; }
    }

    public abstract class LaneConnector : RoadElementBase
    {
        protected LaneConnector( IControl control )
            : base( control )
        {
        }
    }
}