using System.Collections.Generic;
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
        private readonly IVertexContainer<VertexPositionColor> _concretVertexContainer;
        private readonly IMouseHandler _mouseSupport;
        private readonly IControl _parent;

        public MovableRectangle( Factories.Factories factories, Vector2 leftTop, float width, float height, MovablePoint parent )
            : this( factories, parent )
        {
            this.LeftTop = new MovablePoint( factories, leftTop, this );
            this.RightTop = new MovablePoint( factories, leftTop + new Vector2( width, 0 ), this );
            this.RightBottom = new MovablePoint( factories, this.RightTop.Location + new Vector2( 0, height ), this );
            this.LeftBottom = new MovablePoint( factories, this.RightBottom.Location + new Vector2( -width, 0 ), this );
        }

        public MovableRectangle( Factories.Factories factories, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, IControl parent )
            : this( factories, parent )
        {
            this.LeftTop = new MovablePoint( factories, leftTop, this );
            this.RightTop = new MovablePoint( factories, rightTop, this );
            this.RightBottom = new MovablePoint( factories, rightBottom, this );
            this.LeftBottom = new MovablePoint( factories, leftBottom, this );
        }

        public MovableRectangle( Factories.Factories factories, MovablePoint leftTop, MovablePoint rightTop, MovablePoint rightBottom, MovablePoint leftBottom, IControl parent )
            : this( factories, parent )
        {
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.RightBottom = rightBottom;
            this.LeftBottom = leftBottom;
        }

        private MovableRectangle( Factories.Factories factories, IControl parent )
        {
            this._parent = parent;
            this._mouseSupport = factories.MouseHandlerFactory.Create(this);
            this._points = new MovablePoint[ Corners.Count ];
            this._concretVertexContainer = new MovableRectlangeVertexContainer( this );
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

        public override IVertexContainer VertexContainer
        {
            get { return this._concretVertexContainer; }
        }

        public override IMouseHandler MouseSupport
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

        public override void Translate( Matrix matrixTranslation )
        {
            this.Points.ForEach( s =>
                                    {
                                        s.Translate( matrixTranslation );
                                        s.Redraw();
                                    } );
        }
    }
}