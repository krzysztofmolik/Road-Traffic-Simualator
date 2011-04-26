using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Connectors;

namespace RoadTrafficSimulator.Road.Controls
{
    public class EndRoadLaneEdge : Edge
    {
        private RoadLaneBlock _parrent;
        private readonly IMouseHandler _notMovableMouseHandler;

        public EndRoadLaneEdge( Factories.Factories factories, RoadLaneBlock parent )
            : base( factories )
        {
            this._parrent = parent;
            this._notMovableMouseHandler = factories.MouseHandlerFactory.CreateEmpty();
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public EndRoadLaneEdge( Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint, float width, RoadLaneBlock parent )
            : base( factories, startPoint, endPoint )
        {
            this._parrent = parent;
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get { return this._parrent; }
        }

        public override IControl Parent
        {
            get { return this._parrent; }
            set
            {
                if( (value is RoadLaneBlock) ==false ) { throw new ArgumentException("Only RoadLaneBlock is valid"); }
                this._parrent = ( RoadLaneBlock ) value;
            }
        }

        public EndRoadLaneEdgeConnector Connector
        {
            get;
            private set;
        }

        public override Infrastructure.Mouse.IMouseHandler MouseHandler
        {
            get { return this._notMovableMouseHandler; }
        }

        public EndRoadLaneEdge GetOppositeEdge()
        {
            if ( this._parrent.LeftEdge == this )
            {
                return this._parrent.RightEdge;
            }

            // else
            return this._parrent.LeftEdge;
        }
    }
}