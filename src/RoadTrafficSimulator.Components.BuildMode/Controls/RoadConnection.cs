using Common.Xna;
using Microsoft.FSharp.Core;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RoadConnection : Edge, IEdgeLine
    {
        private readonly RoadConnectionConnector _connector;
        private readonly Routes _routes = new Routes();

        public RoadConnection( Factories.Factories factories, Vector2 location, IControl parent )
            : base( factories, Styles.NormalStyle )
        {
            this.Parent = this;
            this._connector = new RoadConnectionConnector( this );
            this.StartPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.EndPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.LeftEdge = new NormalPointEdgeAdapter( this, this );
            this.RightEdge = new InvertPointEdgeAdapter( this, this );
        }

        public RoadConnectionConnector Connector
        {
            get { return this._connector; }
        }

        public override IControl Parent { get; set; }
        public Routes Routes { get { return this._routes; } }
        public NormalPointEdgeAdapter LeftEdge { get; private set; }

        public InvertPointEdgeAdapter RightEdge { get; private set; }

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

            var line = calculator.Calculate( prevLocation, this.StartLocation, nextLocation );
            this.EndPoint.SetLocation( line.End );
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

            var line = calculator.Calculate( prevLocation, this.EndLocation, nextLocation );
            this.StartPoint.SetLocation( line.Start );
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
            this.StartPoint.SetLocation( line.Start );
            this.EndPoint.SetLocation( line.End );
        }
    }
}