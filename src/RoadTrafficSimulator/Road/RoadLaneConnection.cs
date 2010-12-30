using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.RoadJoiners;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaVs10.Utils;

namespace RoadTrafficSimulator.Road
{
    public class EdgeRoadLaneConnection : EdgeBase
    {
        private IConnectionCompositeSupport _connectionSupport;
        public EdgeRoadLaneConnection( Vector2 location, IControl parent )
            : base( parent )
        {
            //this._connectionSupport = new RoadLaneConnectionSupport();
            var startPoint = location + new Vector2( 0, Constans.RoadHeight / 2 );
            var endPoint = location - new Vector2( 0, Constans.RoadHeight / 2 );
            this.StartPoint = new MovablePoint( startPoint, this );
            this.EndPoint = new MovablePoint( endPoint, this );
        }

        public override IConnectionCompositeSupport ConnectionSupport
        {
            get { return this._connectionSupport; }
        }
    }

    public class RoadLaneConnection : SingleControl<VertexPositionColor>
    {
        private readonly Line _line;
        private readonly RoadLaneConnectionVertexContainer _vertexContainer;
        private readonly ControlMouseSupport _mouseSupport;
        private readonly DefaultControlSelectionSupport _sellectionSupport;
        private readonly RoadLaneConnectionSupport _connectionSupport;

        public RoadLaneConnection( Vector2 location, IControl parent )
            : base( parent )
        {
            this._line = new Line(
                                location + new Vector2( 0, Constans.RoadHeight / 2 ),
                                location - new Vector2( 0, Constans.RoadHeight / 2 ) );

            this._mouseSupport = new ControlMouseSupport( this );
            this._vertexContainer = new RoadLaneConnectionVertexContainer( this );
            this._sellectionSupport = new DefaultControlSelectionSupport( this );
            this._connectionSupport = new RoadLaneConnectionSupport( this );
        }

        public Line Line { get { return this._line; } }

        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
        {
            get { return this._vertexContainer; }
        }

        public override IMouseSupport MouseSupport
        {
            get { return this._mouseSupport; }
        }

        public override Vector2 Location
        {
            get
            {
                var line = this._line.End - this._line.Begin;
                return this._line.Begin + ( line / 2 );
            }
        }

        public override ISelectionSupport SelectionSupport
        {
            get { return this._sellectionSupport; }
        }

        public override IConnectionSupport ConnectionSupport
        {
            get { return this._connectionSupport; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this._line.Begin = Vector2.Transform( this._line.Begin, matrixTranslation );
            this._line.End = Vector2.Transform( this._line.End, matrixTranslation );
            this.ChangedSubject.OnNext( new Unit() );
        }
    }
}