using System.Collections.Generic;
using System.Linq;
using Common.Xna;
using Microsoft.FSharp.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RoadConnection : CompositControl<VertexPositionColor>, IEdgeLine, IRouteElement
    {
        private readonly RoadConnectionConnector _connector;

        public RoadConnection( Factories.Factories factories, Vector2 location )
        {
            this._connector = new RoadConnectionConnector( this );
            this.Edge = new NormalEdge( factories, this, location );
            this.RightEdge = new InvertPointEdgeAdapter( this, this.Edge, this );

            this.Edge.StartPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.Edge.EndPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
        }

        public RoadConnectionConnector Connector
        {
            get { return this._connector; }
        }

        public NormalEdge Edge { get; private set; }

        public InvertPointEdgeAdapter RightEdge { get; private set; }

        public override IVertexContainer VertexContainer
        {
            get { return this.Edge.VertexContainer; }
        }

        public override IMouseHandler MouseHandler
        {
            get { return this.Edge.MouseHandler; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.Edge.Translate( matrixTranslation );
            this.OnInvalidate();
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            this.Edge.TranslateWithoutNotification( translationMatrix );
        }

        public IEnumerable<IRouteElement> GetConnectedControls()
        {
            return new[] { this.Connector.NextConnectedEdge.Parent, this.Connector.PreviousConnectedEdge.Parent };
        }

        public override Vector2 Location
        {
            get { return this.Edge.Location; }
            set
            {
                this.Edge.Location = value;
                this.OnInvalidate();
            }
        }

        protected override void OnInvalidate()
        {
            base.OnInvalidate();
            this.RecalculatePosition();
        }

        public void RecalculatePostitionAroundStartPoint()
        {
            //            var calculator = new Calculation2( PointRotation.Start, Constans.RoadHeight );
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight * 2 );
            var prevLocation = this.Connector.OpositeToPreviousEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.OpositeToPreviousEdge.StartLocation )
                                   : FSharpOption<Vector2>.None;

            var nextLocation = this.Connector.OpositeToNextEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.OpositeToNextEdge.StartLocation )
                                   : FSharpOption<Vector2>.None;

            var line = calculator.Calculate( prevLocation, this.Edge.StartLocation, nextLocation );
            this.Edge.EndPoint.SetLocation( line.End );
        }

        public void RecalculatePostitionAroundEndPoint()
        {
            //            var calculator = new Calculation2( PointRotation.End, Constans.RoadHeight );
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight * 2 );
            var prevLocation = this.Connector.OpositeToPreviousEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.OpositeToPreviousEdge.EndLocation )
                                   : FSharpOption<Vector2>.None;

            var nextLocation = this.Connector.OpositeToNextEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.OpositeToNextEdge.EndLocation )
                                   : FSharpOption<Vector2>.None;

            var line = calculator.Calculate( prevLocation, this.Edge.EndLocation, nextLocation );
            this.Edge.StartPoint.SetLocation( line.Start );
        }

        public void RecalculatePosition()
        {
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight );
            var prevLocation = this.Connector.OpositeToPreviousEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.OpositeToPreviousEdge.Location )
                                   : FSharpOption<Vector2>.None;

            var nextLocation = this.Connector.OpositeToNextEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.OpositeToNextEdge.Location )
                                   : FSharpOption<Vector2>.None;

            var line = calculator.Calculate( prevLocation, this.Location, nextLocation );
            this.Edge.StartPoint.SetLocation( line.Start );
            this.Edge.EndPoint.SetLocation( line.End );
        }
    }
}