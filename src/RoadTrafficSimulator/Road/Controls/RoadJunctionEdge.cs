using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Road.Controls
{
    public class RoadJunctionEdge : Edge
    {
        private readonly RoadJunctionEdgeConnector _roadJunctionEndConnector;

        private readonly RoadJunctionBlock _parent;
        private readonly IVertexContainer<VertexPositionColor> _vertexContainer;

        public RoadJunctionEdge(Factories.Factories factories,  RoadJunctionBlock parent ) 
            : base(factories)
        {
            this._parent = parent;
            this._roadJunctionEndConnector = new RoadJunctionEdgeConnector( this );
            this._vertexContainer = new RoadJunctionEdgeVertexContainer( this );
        }

        public RoadJunctionEdge(Factories.Factories factories,  MovablePoint startPoint, MovablePoint endPoint, float width, RoadJunctionBlock parent )
            : base( factories, startPoint, endPoint, width )
        {
            this._parent = parent;
        }

        public RoadJunctionEdgeConnector Connector
        {
            get { return this._roadJunctionEndConnector; }
        }

        public RoadJunctionBlock RoadJunctionParent
        {
            get { return this._parent; }
        }

        public override sealed IControl Parent
        {
            get { return this._parent; }
        }

        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
        {
            get { return this._vertexContainer; }
        }

        public float Length
        {
            get { return Vector2.Distance( this.StartLocation, this.EndLocation ); }
        }

        public override void Invalidate()
        {
            var endRoadLaneEdge = this.Connector.ConnectedObject.FirstOrDefault() as EndRoadLaneEdge;
            if ( endRoadLaneEdge == null )
            {
                return;
            }

            var parpendicularLine = this.GetParpendicularLineToLane( endRoadLaneEdge );
            this.StartPoint.SetLocation( parpendicularLine.Item2 );
            this.EndPoint.SetLocation( parpendicularLine.Item1 );
        }

        private Tuple<Vector2, Vector2> GetParpendicularLineToLane( EndRoadLaneEdge endRoadLaneEdge )
        {
            var opositeEdge = endRoadLaneEdge.GetOppositeEdge();
            return MyMathHelper.CreatePerpendicualrLine( this.Location, opositeEdge.Location, Constans.RoadHeight );
        }
    }
}