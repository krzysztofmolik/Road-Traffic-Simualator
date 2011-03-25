using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

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
            : base( factories, startPoint, endPoint )
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

        public override IVertexContainer VertexContainer
        {
            get { return this._vertexContainer; }
        }
    }
}