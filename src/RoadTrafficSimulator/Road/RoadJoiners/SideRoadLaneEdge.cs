using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road.RoadJoiners;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public class SideRoadLaneEdge : EdgeBase
    {
        private LaneType _laneType;
        private SideRoadLaneEdgeVertexContainer _vertexContainer;

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
            get { return this._laneType; }
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