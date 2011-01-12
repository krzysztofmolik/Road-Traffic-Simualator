using System;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road.Controls
{
    public class SideRoadLaneEdge : Edge
    {
        private readonly RoadLaneBlock _parent;
        private readonly SideRoadLaneEdgeVertexContainer _vertexContainer;
        private LaneType _laneType;

        public SideRoadLaneEdge( MovablePoint startPoint, MovablePoint endPoint, float width, RoadLaneBlock parent )
            : base( startPoint, endPoint, width )
        {
            this._parent = parent;
            this._laneType = LaneType.SolidLine;
            this._vertexContainer = new SideRoadLaneEdgeVertexContainer( this );
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get
            {
                return this._parent;
            }
        }

        public LaneType LaneType
        {
            get
            {
                return this._laneType;
            }

            set
            {
                this._laneType = value;
                this.ChangedSubject.OnNext( new Unit() );
            }
        }

        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
        {
            get { return this._vertexContainer; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }
    }
}