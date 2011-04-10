using Common.Xna;
using Microsoft.FSharp.Core;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using XnaRoadTrafficConstructor.Road;

namespace RoadTrafficSimulator.Road.Controls
{
    public class CarsInserter : Edge
    {
        private IControl _parent;
        private CarsInsertedConnector _connector;

        public CarsInserter( Factories.Factories factories, Vector2 location, IControl parent )
            : base( factories )
        {
            this._parent = parent;
            this._connector = new CarsInsertedConnector( this );
            this.StartPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.EndPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
        }

        public CarsInserter( Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint )
            : base( factories, startPoint, endPoint ) {  }

        public override IControl Parent { get { return this._parent; } }

        public CarsInsertedConnector Connector { get { return this._connector; } }

        protected override void OnInvalidate()
        {
            this.RecalculatePosition();
            base.OnInvalidate();
        }

        // TODO Remove duplication ... ehhh no :)
        public void RecalculatePostitionAroundStartPoint()
        {
            if ( this.Connector.ConnectedRoad == null )
            {
                return;
            }
            //            var calculator = new Calculation2( PointRotation.Start, Constans.RoadHeight );
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight * 2 );
            var nextLocation = FSharpOption<Vector2>.Some( this.Connector.ConnectedRoad.StartLocation );

            var line = calculator.Calculate( FSharpOption<Vector2>.None, this.StartLocation, nextLocation );
            this.EndPoint.SetLocation( line.End );
        }

        public void RecalculatePostitionAroundEndPoint()
        {
            if ( this.Connector.ConnectedRoad == null )
            {
                return;
            }
            //            var calculator = new Calculation2( PointRotation.End, Constans.RoadHeight );
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight * 2 );
            var nextLocation = FSharpOption<Vector2>.Some( this.Connector.ConnectedRoad.EndLocation );

            var line = calculator.Calculate( FSharpOption<Vector2>.None, this.EndLocation, nextLocation );
            this.StartPoint.SetLocation( line.Start );
        }

        public void RecalculatePosition()
        {
            if ( this.Connector.ConnectedRoad == null )
            {
                return;
            }

            var calculator = new CalculateEdgeAngel( Constans.RoadHeight );
            var nextLocation = FSharpOption<Vector2>.Some( this.Connector.ConnectedRoad.Location );
            var line = calculator.Calculate( FSharpOption<Vector2>.None, this.Location, nextLocation );
            this.StartPoint.SetLocation( line.Start );
            this.EndPoint.SetLocation( line.End );
        }
    }
}