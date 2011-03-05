using Common.Xna;
using Microsoft.FSharp.Core;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using Unit = System.Unit;

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
            this._connector = new RoadConnectionConnector( this );
            this.StartPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.EndPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
        }

        public RoadConnectionConnector Connector
        {
            get { return this._connector; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }

        protected override void OnTranslated()
        {
            base.OnTranslated();
            this.RecalculatePosition();
            this.Connector.NotifyAboutTranslation();
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