using System;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RoadJunctionEdge : Edge, IRoadElement
    {
        private readonly RoadJunctionEdgeConnector _roadJunctionEndConnector;
        private readonly Routes _routes = new Routes();

        private RoadJunctionBlock _parent;
        private readonly IVertexContainer<VertexPositionColor> _vertexContainer;

        public RoadJunctionEdge( Factories.Factories factories, RoadJunctionBlock parent )
            : base( factories, Styles.NormalStyle )
        {
            this._roadJunctionEndConnector = new RoadJunctionEdgeConnector( this );
            this._vertexContainer = new RoadJunctionEdgeVertexContainer( this );
        }

        public RoadJunctionEdge( Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint, float width, RoadJunctionBlock parent )
            : base( factories, startPoint, endPoint, Styles.NormalStyle )
        {
            this._parent = parent;
        }

        public RoadJunctionEdgeConnector Connector
        {
            get { return this._roadJunctionEndConnector; }
        }

        public Routes Routes { get { return this._routes; } }

        public RoadJunctionBlock RoadJunctionParent
        {
            get { return this._parent; }
        }

        public override sealed IControl Parent
        {
            get { return this._parent; }
            set
            {
                if ( ( value is RoadJunctionBlock ) == false ) { throw new ArgumentException( "Only RoadJuntionBlock is allowed" ); }
                this._parent = ( RoadJunctionBlock ) value;
            }
        }

        public override IVertexContainer VertexContainer
        {
            get { return this._vertexContainer; }
        }
    }
}