using Common.Xna;
using Microsoft.FSharp.Core;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Road;

namespace RoadTrafficSimulator.Road
{
    public class RoadConnection : Edge
    {
        private readonly RoadConnectionConnector _connector;
        private readonly IControl _parent;

        public RoadConnection( Factories.Factories factories, Vector2 location, IControl parent )
            : base( factories )
        {
            this._parent = parent;
            this.Order = this._parent.Order + 1;
            this._connector = new RoadConnectionConnector( this );
            this.StartPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.EndPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.LeftEdge = new RoadConnectionEdge( this, shouldInvert: false );
            this.RightEdge = new RoadConnectionEdge( this, shouldInvert: true );
        }

        public RoadConnectionConnector Connector
        {
            get { return this._connector; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }

        public RoadConnectionEdge LeftEdge { get; private set; }

        public RoadConnectionEdge RightEdge { get; private set; }

        protected override void OnTranslated()
        {
            base.OnTranslated();
//            this.RecalculatePosition( this.LeftEdge );
//            this.Connector.NotifyAboutTranslation();
        }

        public void RecalculatePostitionAroundStartPoint()
        {
            var calculator = new Calculation2( PointRotation.Start, Constans.RoadHeight );
            var prevLocation = this.Connector.PreviousEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.PreviousEdge.StartLocation )
                                   : FSharpOption<Vector2>.None;

            var nextLocation = this.Connector.NextEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.NextEdge.StartLocation )
                                   : FSharpOption<Vector2>.None;

            var line = calculator.Calculate( prevLocation, this.StartLocation, nextLocation );
            this.EndPoint.SetLocation( line );
        }

        public void RecalculatePostitionAroundEndPoint()
        {
            var calculator = new Calculation2( PointRotation.End, Constans.RoadHeight );
            var prevLocation = this.Connector.PreviousEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.PreviousEdge.EndLocation )
                                   : FSharpOption<Vector2>.None;

            var nextLocation = this.Connector.NextEdge != null
                                   ? FSharpOption<Vector2>.Some( this.Connector.NextEdge.EndLocation )
                                   : FSharpOption<Vector2>.None;

            var line = calculator.Calculate( prevLocation, this.EndLocation, nextLocation );
            this.StartPoint.SetLocation( line );
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