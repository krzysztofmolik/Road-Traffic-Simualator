using System;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class EndRoadLaneEdge : Edge, IRoadElement, IComponent
    {
        private RoadLaneBlock _parrent;
        private readonly IMouseHandler _notMovableMouseHandler;
        private readonly Routes _routes = new Routes();

        public EndRoadLaneEdge( Factories.Factories factories, RoadLaneBlock parent )
            : base( factories, Styles.NormalStyle, parent )
        {
            this._parrent = parent;
            this._notMovableMouseHandler = factories.MouseHandlerFactory.CreateEmpty();
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public EndRoadLaneEdge( Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint, RoadLaneBlock parent )
            : base( factories, startPoint, endPoint, Styles.NormalStyle, parent )
        {
            this._parrent = parent;
            this._notMovableMouseHandler = factories.MouseHandlerFactory.CreateEmpty();
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public Routes Routes { get { return this._routes; } }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get { return this._parrent; }
        }

        public IControl Parent
        {
            get { return this._parrent; }
            set
            {
                if ( ( value is RoadLaneBlock ) == false ) { throw new ArgumentException( "Only RoadLaneBlock is valid" ); }
                this._parrent = ( RoadLaneBlock ) value;
            }
        }

        public EndRoadLaneEdgeConnector Connector
        {
            get;
            private set;
        }

        public override IMouseHandler MouseHandler
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