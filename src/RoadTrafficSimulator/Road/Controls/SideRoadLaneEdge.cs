using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road.Controls
{
    public class SideRoadLaneEdge : Edge
    {
        private readonly SideRoadLaneEdgeVertexContainer _vertexContainer;
        private LaneType _laneType;

        public SideRoadLaneEdge( MovablePoint startPoint, MovablePoint endPoint, float width, RoadLaneBlock parent )
            : base( startPoint, endPoint, width, parent )
        {
            this._laneType = LaneType.SolidLine;
            this._vertexContainer = new SideRoadLaneEdgeVertexContainer( this );
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get
            {
                var result = this.Parents.First() as RoadLaneBlock;
                Debug.Assert( result != null, "result != null" );
                return result;
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
    }
}