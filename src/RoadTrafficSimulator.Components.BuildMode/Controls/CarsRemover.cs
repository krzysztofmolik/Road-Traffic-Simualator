using System;
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
    public class CarsRemover : CompositControl<VertexPositionColor>, IEdgeLine
    {
        private readonly CarsRemoverConnector _connector;
        private readonly NormalEdge _edge;

        public CarsRemover( Factories.Factories factories, Vector2 location )
        {
            this._connector = new CarsRemoverConnector( this );
            this._edge = new NormalEdge( factories, this, location );
            this._edge.Translated.Subscribe( _ => this.Invalidate() );
        }

        public NormalEdge Edge { get { return this._edge; }}


        public CarsRemoverConnector Connector { get { return this._connector; } }

        public override IVertexContainer VertexContainer
        {
            get { return this._edge.VertexContainer; }
        }

        public override IMouseHandler MouseHandler
        {
            get { return this._edge.MouseHandler; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this._edge.Translate( matrixTranslation );
            this.Invalidate();
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            this._edge.TranslateWithoutNotification( translationMatrix );
        }

        public override Vector2 Location
        {
            get { return this._edge.Location; }
            set
            {
                this._edge.Location = value;
                this.Invalidate();
            }
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
            var prevLocation = FSharpOption<Vector2>.Some( this.Connector.OpositeEdge.StartLocation );

            var line = calculator.Calculate( prevLocation, this._edge.StartLocation, FSharpOption<Vector2>.None );
            this._edge.EndPoint.SetLocation( line.End );
        }

        public void RecalculatePostitionAroundEndPoint()
        {
            if ( this.Connector.OpositeEdge == null ) { return; }
            //            var calculator = new Calculation2( PointRotation.End, Constans.RoadHeight );
            var calculator = new CalculateEdgeAngel( Constans.RoadHeight * 2 );
            var prevLocation = FSharpOption<Vector2>.Some( this.Connector.OpositeEdge.EndLocation );

            var line = calculator.Calculate( prevLocation, this._edge.EndLocation, FSharpOption<Vector2>.None );
            this._edge.StartPoint.SetLocation( line.Start );
        }

        public void RecalculatePosition()
        {
            if ( this.Connector.OpositeEdge == null ) { return; }

            var calculator = new CalculateEdgeAngel( Constans.RoadHeight );
            var prevLocation = FSharpOption<Vector2>.Some( this.Connector.OpositeEdge.Location );
            var line = calculator.Calculate( prevLocation, this.Location, FSharpOption<Vector2>.None );
            this._edge.StartPoint.SetLocation( line.Start );
            this._edge.EndPoint.SetLocation( line.End );
        }
    }
}