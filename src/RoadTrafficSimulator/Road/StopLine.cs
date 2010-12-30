using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.VertexContainers;
using XnaVs10.MathHelpers;
using XnaVs10.Utils;

namespace RoadTrafficSimulator.Road
{
    public class StopLine : SingleControl<VertexPositionColor>
    {
        private readonly IRoadLaneBlock _parent;

        private readonly StopLineVertexContainer _stopLineVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly ISelectionSupport _selectionSupport;

        private Vector2[] _shape;
        private IConnectionSupport _connectionSupport;

        public StopLine( IRoadLaneBlock parent ) : base(parent)
        {
            this._mouseSupport = new ControlMouseSupport(this);
            EnsureThatParamterIsValid( parent );

            this._parent = parent;
            this._parent.VectorChanged += ( sender, arg ) => this.UpdateLocation();
            this.Shape = new Vector2[ 0 ];
            this.UpdateLocation();

            this._stopLineVertexContainer = new StopLineVertexContainer( this );
            this._selectionSupport = new DefaultControlSelectionSupport(this);
            this._connectionSupport = new EmptyConnectionSupport<StopLine>( this );
        }

        public Vector2[] Shape
        {
            get { return this._shape; }
            private set
            {
                this._shape = value;
                this.ChangedSubject.OnNext( new Unit() );
            }
        }

        public Vector2 LeftTop { get { return this._shape[ 0 ]; } }

        public Vector2 RightTop { get { return this._shape[ 1 ]; } }

        public Vector2 RightBottom { get { return this._shape[ 2 ]; } }

        public Vector2 LeftBottom { get { return this._shape[ 3 ]; } }


        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
        {
            get { return this._stopLineVertexContainer; }
        }

        public override IMouseSupport MouseSupport
        {
            get { return this._mouseSupport; }
        }

        public override IConnectionSupport ConnectionSupport
        {
            get { return this._connectionSupport; }
        }

        public override Vector2 Location
        {
            get
            {
                Debug.Assert(this.Shape.Length > 0, "this.Shape.Length > 0");
                return this.Shape.First();
            }
        }

        public override ISelectionSupport SelectionSupport
        {
            get { return this._selectionSupport; }
        }

        public void UpdateLocation()
        {
            var centerVector = this._parent.RightTopLocation - this._parent.LeftTopLocation;
            centerVector *= 0.5f;
            centerVector += this._parent.LeftTopLocation;

            var leftDistance = Math.Abs( ( centerVector - this._parent.LeftBottomLocation ).Length() );
            var rightDistance = Math.Abs( ( centerVector - this._parent.RightBottomLocation ).Length() );
            if ( leftDistance < rightDistance )
            {
                var reflectLine = new Line( this._parent.LeftBottomLocation, this._parent.LeftTopLocation );
                this.CreateLine( reflectLine );
            }
            else
            {
                var reflectLine = new Line( this._parent.RightBottomLocation, this._parent.RightTopLocation );
                this.CreateLine( reflectLine );
            }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.Shape = this._shape.Select( s => Vector2.Transform( s, matrixTranslation ) ).ToArray();
        }

        private void CreateLine( Line baseLine )
        {
            var parpendicualLine = MyMathHelper.CreatePerpendicualrLine( baseLine, 5 );
            var leftIntersectionPoint = MyMathHelper.LineIntersectionMethod(
                parpendicualLine,
                Tuple.Create( this._parent.LeftTopLocation, this._parent.LeftBottomLocation ) );
            var rightIntersetionPoint = MyMathHelper.LineIntersectionMethod(
                parpendicualLine,
                Tuple.Create( this._parent.RightTopLocation, this._parent.RightBottomLocation ) );

            this.Shape = CreateShape( leftIntersectionPoint, rightIntersetionPoint );
        }

        private static Vector2[] CreateShape( Vector2 begin, Vector2 end )
        {
            var moveVector = MyMathHelper.CreatePerpendicularVector( ( begin - end ), Constans.LaneWidth / 2 );

            return new[]
                       {
                           begin + moveVector,  // left top
                           end + moveVector,    // right top
                           end - moveVector,    // right bottom
                           begin - moveVector   // right bottom
                       };
        }

        private static void EnsureThatParamterIsValid( IRoadLaneBlock parent )
        {
            //TODO
            //            if ( !parent.LeftLine.IsValid() || !parent.RightLine.IsValid() )
            //            {
            //                throw new ArgumentException( "Road line is not valid" );
            //            }
            Debug.Assert( parent.LeftTopLocation != parent.LeftBottomLocation );
            Debug.Assert( parent.RightTopLocation != parent.RightBottomLocation );
        }
    }
}