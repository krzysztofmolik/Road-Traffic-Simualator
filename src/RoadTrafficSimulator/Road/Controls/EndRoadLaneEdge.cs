using System;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Road.Controls
{
    public class EndRoadLaneEdge : Edge
    {
        private readonly RoadLaneBlock _parrent;

        public EndRoadLaneEdge(Factories.Factories factories,  RoadLaneBlock parent ) 
            : base(factories)
        {
            this._parrent = parent;
        }

        public EndRoadLaneEdge(Factories.Factories factories,  MovablePoint startPoint, MovablePoint endPoint, float width, RoadLaneBlock parent )
            : base( factories, startPoint, endPoint, width )
        {
            this._parrent = parent;
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get { return this._parrent; }
        }

        public override IControl Parent
        {
            get { return this._parrent; }
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