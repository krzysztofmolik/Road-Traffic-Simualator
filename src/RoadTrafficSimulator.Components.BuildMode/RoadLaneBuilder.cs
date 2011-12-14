using System;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class RoadLaneBuilder
    {
        private readonly Func<RoadLaneBlock> _roadLaneBlockFactory;
        private readonly Func<Vector2, RoadConnection> _roadConnectionEdgeFactory;
        private readonly CompositeConnectionCommand _connectionCommand;
        private IControl _lastConnectedControl;
        private ICompositeControl _owner;

        public RoadLaneBuilder(
            Func<RoadLaneBlock> roadLaneBlockFactory,
            Func<Vector2, RoadConnection> roadConnectionEdgeFactory,
            CompositeConnectionCommand connectionCommand )
        {
            this._roadLaneBlockFactory = roadLaneBlockFactory;
            this._roadConnectionEdgeFactory = roadConnectionEdgeFactory;
            this._connectionCommand = connectionCommand;
        }

        public void Clear()
        {
            this._lastConnectedControl = null;
        }

        public void StartFrom( IControl control )
        {
            if ( this._lastConnectedControl != null ) { throw new InvalidOperationException(); }

            this._lastConnectedControl = control.NotNull();
        }

        public void CreateBlockTo( Vector2 location )
        {
            var roadLane = this.CreateRoadLane();
            this._connectionCommand.Connect( this._lastConnectedControl, roadLane );

            var roadLaneConnection = this.CreateRoadLaneConnection( location );
            this._connectionCommand.Connect( roadLane, roadLaneConnection );

            this._owner.AddChild( roadLane );

            this._lastConnectedControl = roadLaneConnection;
        }

        public void EndIn( IControl lastControl )
        {
            var roadLane = this.CreateRoadLane();
            this._connectionCommand.Connect( this._lastConnectedControl, roadLane );
            this._connectionCommand.Connect( roadLane, lastControl );

            this._owner.AddChild( roadLane );
            this._lastConnectedControl = null;
        }

        public void SetOwner( ICompositeControl owner )
        {
            this._owner = owner.NotNull();
        }

        private RoadLaneBlock CreateRoadLane()
        {
            return this._roadLaneBlockFactory();
        }

        private RoadConnection CreateRoadLaneConnection( Vector2 location )
        {
            var roadLaneConnection = this._roadConnectionEdgeFactory( location );
            this._owner.AddChild( roadLaneConnection );
            return roadLaneConnection;
        }
    }
}