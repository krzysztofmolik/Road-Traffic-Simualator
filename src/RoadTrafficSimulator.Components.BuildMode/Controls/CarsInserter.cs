using System;
using System.Collections.Generic;
using System.Linq;
using Common.Xna;
using Microsoft.FSharp.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class CarsInserter : CompositControl<VertexPositionColor>, IRouteOwner, IEdgeLine, IRouteElement
    {
        private readonly CarsInsertedConnector _connector;
        private readonly Routes _routes = new Routes();
        private uint _carInsertInterval;

        public CarsInserter( Factories.Factories factories, Vector2 location )
        {
            this._connector = new CarsInsertedConnector( this );
            this.Edge = new NormalEdge( factories, this, location );
            this.RightEdge = new InvertPointEdgeAdapter( this, this.Edge, this );
            this.Edge.Translated.Subscribe( _ => this.Invalidate() );
            this.Edge.VertexContainer.Color = Color.Green;
            this._carInsertInterval = 2000;
        }

        public InvertPointEdgeAdapter RightEdge { get; private set; }
        public Edge Edge { get; private set; }

        public Routes Routes { get { return this._routes; } }

        public CarsInsertedConnector Connector { get { return this._connector; } }

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
            this.Invalidate();
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            this.Edge.TranslateWithoutNotification( translationMatrix );
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

        public void SetCarInsertInterval( uint value )
        {
            this._carInsertInterval = value;
        }

        public uint GetCarInsertInterval()
        {
            return this._carInsertInterval;
        }

        protected override void OnInvalidate()
        {
            this.RecalculatePosition();
            base.OnInvalidate();
        }

        // TODO Remove duplication ... ehhh no :)
        public void RecalculatePostitionAroundStartPoint()
        {
            if ( this.Connector.OpositeEdge == null ) { return; }
            //            var calculator = new Calculation2( PointRotation.Start, Constans.RoadHeight );
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight * 2 );
            var nextLocation = FSharpOption<Vector2>.Some( this.Connector.OpositeEdge.StartLocation );

            var line = calculator.Calculate( FSharpOption<Vector2>.None, this.Edge.StartLocation, nextLocation );
            this.Edge.EndPoint.SetLocation( line.End );
        }

        public void RecalculatePostitionAroundEndPoint()
        {
            if ( this.Connector.OpositeEdge == null )
            {
                return;
            }
            //            var calculator = new Calculation2( PointRotation.End, Constans.RoadHeight );
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight * 2 );
            var nextLocation = FSharpOption<Vector2>.Some( this.Connector.OpositeEdge.EndLocation );

            var line = calculator.Calculate( FSharpOption<Vector2>.None, this.Edge.EndLocation, nextLocation );
            this.Edge.StartPoint.SetLocation( line.Start );
        }

        public void RecalculatePosition()
        {
            if ( this.Connector.OpositeEdge == null )
            {
                return;
            }

            var calculator = new CalculateEdgeAngel( Constans.RoadHeight );
            var nextLocation = FSharpOption<Vector2>.Some( this.Connector.OpositeEdge.Location );
            var line = calculator.Calculate( FSharpOption<Vector2>.None, this.Location, nextLocation );
            this.Edge.StartPoint.SetLocation( line.Start );
            this.Edge.EndPoint.SetLocation( line.End );
        }

        public IEnumerable<IRouteElement> GetConnectedControls()
        {
            return new[] { this.Connector.ConnectedEdge.Parent };
        }
    }
}