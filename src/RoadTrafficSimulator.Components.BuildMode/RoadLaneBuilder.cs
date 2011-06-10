using System;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class RoadLaneBuilder
    {
        private readonly Func<ICompositeControl, IRoadLaneBlock> _roadLaneBlockFactory;
        private readonly Func<Vector2, ICompositeControl, RoadConnection> _roadConnectionEdgeFactory;
        private readonly CompositeConnectionCommand _connectionCommand;
        private IControl _lastConnectedControl;
        private ICompositeControl _owner;

        public RoadLaneBuilder(
            Func<ICompositeControl, IRoadLaneBlock> roadLaneBlockFactory,
            Func<Vector2, ICompositeControl, RoadConnection> roadConnectionEdgeFactory,
            CompositeConnectionCommand connectionCommand )
        {
            this._roadLaneBlockFactory = roadLaneBlockFactory;
            this._roadConnectionEdgeFactory = roadConnectionEdgeFactory;
            this._connectionCommand = connectionCommand;
        }

        public void StartFrom( IControl control )
        {
            if ( this._lastConnectedControl != null ) { throw new InvalidOperationException(); }

            this._lastConnectedControl = control.NotNull();
        }

        public void CreateBlockTo( Vector2 location )
        {
            var roadLane = this.CreateRoadLane();
            this._connectionCommand.Connect( this._lastConnectedControl, roadLane.LeftEdge );

            var roadLaneConnection = this.CreateRoadLaneConnection( location );
            this._connectionCommand.Connect( roadLane.RightEdge, roadLaneConnection );

            this._owner.AddChild( roadLane );

            this._lastConnectedControl = roadLaneConnection;
        }

        public void EndIn( IControl lastControl )
        {
            var roadLane = this.CreateRoadLane();
            this._connectionCommand.Connect( this._lastConnectedControl, roadLane.LeftEdge );
            this._connectionCommand.Connect( roadLane.RightEdge, lastControl );

            this._owner.AddChild( roadLane );
            this._lastConnectedControl = null;
        }

        public void SetOwner( ICompositeControl owner )
        {
            this._owner = owner.NotNull();
        }

        private IRoadLaneBlock CreateRoadLane()
        {
            return this._roadLaneBlockFactory( this._owner );
        }

        private RoadConnection CreateRoadLaneConnection( Vector2 location )
        {
            var roadLaneConnection = this._roadConnectionEdgeFactory( location, this._owner );
            this._owner.AddChild( roadLaneConnection );
            return roadLaneConnection;
        }
    }
}