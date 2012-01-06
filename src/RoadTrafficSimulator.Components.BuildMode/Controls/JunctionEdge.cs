using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class JunctionEdge : CompositControl<VertexPositionColor>, IRouteElement
    {
        private readonly NormalEdge _edge;
        private readonly JunctionEdgeConnector _connector;
        private readonly IEdge _invertedEdge;

        public JunctionEdge( Factories.Factories factories, Vector2 location )
        {
            this._edge = new NormalEdge( factories, this, location );
            this._connector = new JunctionEdgeConnector( this );
            this._invertedEdge = new InvertedEdgeAdapter( this._edge, this );
        }

        public NormalEdge Edge { get { return this._edge; } }

        public JunctionEdgeConnector Connector { get { return this._connector; } }

        public override IVertexContainer VertexContainer
        {
            get { return this._edge.VertexContainer; }
        }

        public override IMouseHandler MouseHandler
        {
            get { return this.Edge.MouseHandler; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.Edge.Translate( matrixTranslation );
            this.Invalidate();
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            this.Edge.TranslateWithoutNotification( translationMatrix );
        }

        public IEnumerable<IRouteElement> GetConnectedControls()
        {
            if ( this.Connector.Edge != null )
            {
                return new[] { this.Connector.JunctionEdge.Parent, this.Connector.Edge.Parent };
            }

            return new[] { this.Connector.JunctionEdge.Parent };
        }

        public override Vector2 Location
        {
            get { return this.Edge.Location; }
            set
            {
                this.Edge.Location = value;
                this.Invalidate();
            }
        }

        public IEdge InvertedEdge { get { return this._invertedEdge; } }
    }
}