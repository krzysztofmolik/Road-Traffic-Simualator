using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Road.RoadJoiners;
using XnaRoadTrafficConstructor.Road.RoadJoiners;

namespace RoadTrafficSimulator.Road
{
    public class RoadLaneCreator
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly ISubject<IRoadLaneBlock> _roadLaneCreated = new Subject<IRoadLaneBlock>();
        private readonly VisitAllChildren _visitator;
        private readonly CompositeConnectionCommand _connectionCommand;
        private IControl _lastSelectedControl;

        private IControl _roadOwner;

        public RoadLaneCreator( IMouseInformation mouseInformation, VisitAllChildren visitator, CompositeConnectionCommand connectionCommand )
        {
            this._mouseInformation = mouseInformation;
            this._connectionCommand = connectionCommand;
            this._visitator = visitator;

            this._mouseInformation.LeftButtonPressed.Subscribe( this.MousePressed );
        }

        public IControl RoadOwner
        {
            get { return this._roadOwner; }
            set { this._roadOwner = value.NotNull(); }
        }

        public IObservable<IRoadLaneBlock> RoadCreated { get { return this._roadLaneCreated; } }

        public void Begin( IControl owner )
        {
            this.RoadOwner = owner;
            this._mouseInformation.StartRecord();
        }

        public void End()
        {
            this._mouseInformation.StopRecord();
        }

        private void MousePressed( XnaMouseState mouseState )
        {
            var edge = this.GetEdgeAtPoint( mouseState.Location );

            if ( edge == null && this._lastSelectedControl != null )
            {
                var connection = this.CreateRoadLaneConnection( mouseState.Location );
                this._connectionCommand.Connect( this._lastSelectedControl, connection );
                this._lastSelectedControl = connection;
                return;
            }

            if ( edge == null )
            {
                return;
            }

            var edgeParent = this.GetParent<IRoadJunctionBlock>( edge );
            if ( edgeParent == null )
            {
                return;
            }

            if ( this._lastSelectedControl == null )
            {
                this._lastSelectedControl = edge;
                return;
            }

            this.CreateRoadBetween( this._lastSelectedControl, edge );
            this._lastSelectedControl = null;
        }

        private RoadLaneConnection CreateRoadLaneConnection( Vector2 location )
        {
            return new RoadLaneConnection( location, this._roadOwner );
        }

        private void CreateRoadBetween( IControl first, IControl second )
        {
            var roadLane = new RoadLaneBlock( this.RoadOwner );
            if ( this._connectionCommand.Connect( roadLane.LeftEdge, first ) == false )
            {
                return;
            }

            if ( this._connectionCommand.Connect( roadLane.RightEdge, second ) == false )
            {
                return;
            }

            this._roadLaneCreated.OnNext( roadLane );
        }

        private TParent GetParent<TParent>( IControl edge )
        {
            return edge.Parents.OfType<TParent>().FirstOrDefault();
        }

        private RoadJunctionEdge GetEdgeAtPoint( Vector2 location )
        {
            return this._visitator.Where( c => c.HitTest( location ) ).OfType<RoadJunctionEdge>().FirstOrDefault();
        }
    }
}