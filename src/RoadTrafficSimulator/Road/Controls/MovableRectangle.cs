using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.VertexContainers;

namespace RoadTrafficSimulator.Road.Controls
{
    public class MovableRectangle : CompostControl<VertexPositionColor>
    {
        private readonly MovablePoint[] _points;
        private readonly IVertexContainer<VertexPositionColor> _specifiedVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly ISelectionSupport _selectionSupport;
        private readonly IControl _parent;

        public MovableRectangle( Vector2 leftTop, float width, float height, MovablePoint parent )
            : this( parent )
        {
            this.LeftTop = new MovablePoint( leftTop, this );
            this.RightTop = new MovablePoint( leftTop + new Vector2( width, 0 ), this );
            this.RightBottom = new MovablePoint( this.RightTop.Location + new Vector2( 0, height ), this );
            this.LeftBottom = new MovablePoint( this.RightBottom.Location + new Vector2( -width, 0 ), this );
        }

        public MovableRectangle( Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, IControl parent )
            : this( parent )
        {
            this.LeftTop = new MovablePoint( leftTop, this );
            this.RightTop = new MovablePoint( rightTop, this );
            this.RightBottom = new MovablePoint( rightBottom, this );
            this.LeftBottom = new MovablePoint( leftBottom, this );
        }

        public MovableRectangle( MovablePoint leftTop, MovablePoint rightTop, MovablePoint rightBottom, MovablePoint leftBottom, IControl parent )
            : this( parent )
        {
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.RightBottom = rightBottom;
            this.LeftBottom = leftBottom;
        }

        private MovableRectangle( IControl parent )
        {
            this._parent = parent;
            this._mouseSupport = new CompositeControlMouseSupport( this );
            this._points = Enumerable.Range( 0, Corners.Count ).Select( i => new MovablePoint( Vector2.Zero, this ) ).ToArray();
            this._specifiedVertexContainer = new MovableRectlangeVertexContainer( this );
            this._selectionSupport = new DefaultCompositeControlSelectionSupport( this );
        }

        public IEnumerable<MovablePoint> Points
        {
            get { return this._points; }
        }

        public MovablePoint LeftBottom
        {
            get
            {
                return this._points[ Corners.LeftBottom ];
            }

            set
            {
                this.RemoveChild( this._points[ Corners.LeftBottom ] );
                this._points[ Corners.LeftBottom ] = value;
                this.AddChild( value );
            }
        }

        public MovablePoint RightBottom
        {
            get
            {
                return this._points[ Corners.RightBottom ];
            }

            set
            {
                this.RemoveChild( this._points[ Corners.RightBottom ] );
                this._points[ Corners.RightBottom ] = value;
                this.AddChild( value );
            }
        }

        public MovablePoint RightTop
        {
            get
            {
                return this._points[ Corners.RightTop ];
            }

            set
            {
                this.RemoveChild( this._points[ Corners.RightTop ] );
                this._points[ Corners.RightTop ] = value;
                this.AddChild( value );
            }
        }

        public MovablePoint LeftTop
        {
            get
            {
                return this._points[ Corners.LeftTop ];
            }

            set
            {
                this.RemoveChild( this._points[ Corners.LeftTop ] );
                this._points[ Corners.LeftTop ] = value;
                this.AddChild( value );
            }
        }

        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
        {
            get { return this._specifiedVertexContainer; }
        }

        public override IMouseSupport MouseSupport
        {
            get { return this._mouseSupport; }
        }

        public override Vector2 Location
        {
            get { return this.LeftTop.Location; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }

        public override ISelectionSupport SelectionSupport
        {
            get { return this._selectionSupport; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.Points.ForEach( s => s.Translate( matrixTranslation ) );
        }
    }
}