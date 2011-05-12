using Common.Xna;
using Microsoft.FSharp.Core;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RoadConnection : Edge, IEdgeLine
    {
        private readonly RoadConnectionConnector _connector;

        public RoadConnection( Factories.Factories factories, Vector2 location, IControl parent )
            : base( factories )
        {
            this.Parent = parent;
            this._connector = new RoadConnectionConnector( this );
            this.StartPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.EndPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.LeftEdge = new NormalPointEdgeAdapter( this );
            this.RightEdge = new InvertPointEdgeAdapter( this );
        }

        public RoadConnectionConnector Connector
        {
            get { return this._connector; }
        }

        public override IControl Parent { get; set; }

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
            var prevLocation = this.Connector.PreviousEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.PreviousEdge.StartLocation )
                                   : FSharpOption<Vector2>.None;

            var nextLocation = this.Connector.NextEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.NextEdge.StartLocation )
                                   : FSharpOption<Vector2>.None;

            var line = calculator.Calculate( prevLocation, this.StartLocation, nextLocation );
            this.EndPoint.SetLocation( line.End );
        }

        public void RecalculatePostitionAroundEndPoint()
        {
            //            var calculator = new Calculation2( PointRotation.End, Constans.RoadHeight );
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight * 2 );
            var prevLocation = this.Connector.PreviousEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.PreviousEdge.EndLocation )
                                   : FSharpOption<Vector2>.None;

            var nextLocation = this.Connector.NextEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.NextEdge.EndLocation )
                                   : FSharpOption<Vector2>.None;

            var line = calculator.Calculate( prevLocation, this.EndLocation, nextLocation );
            this.StartPoint.SetLocation( line.Start );
        }

        public void RecalculatePosition()
        {
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight );
            var prevLocation = this.Connector.PreviousEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.PreviousEdge.Location )
                                   : FSharpOption<Vector2>.None;

            var nextLocation = this.Connector.NextEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.NextEdge.Location )
                                   : FSharpOption<Vector2>.None;

            var line = calculator.Calculate( prevLocation, this.Location, nextLocation );
            this.StartPoint.SetLocation( line.Start );
            this.EndPoint.SetLocation( line.End );
        }
    }
}