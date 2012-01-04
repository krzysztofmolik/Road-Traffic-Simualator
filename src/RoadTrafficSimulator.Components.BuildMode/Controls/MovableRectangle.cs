using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    // TODO REmove it ??
    public class MovableRectangle : CompositControl<VertexPositionColor>, IComponent
    {
        private readonly MovablePoint[] _points;
        private readonly IVertexContainer<VertexPositionColor> _concretVertexContainer;
        private readonly IMouseHandler _mouseHandler;

        public MovableRectangle( Factories.Factories factories, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, IRouteElement parent )
            : this( factories, parent )
        {
            this.LeftTop = new MovablePoint( factories, leftTop, this );
            this.RightTop = new MovablePoint( factories, rightTop, this );
            this.RightBottom = new MovablePoint( factories, rightBottom, this );
            this.LeftBottom = new MovablePoint( factories, leftBottom, this );
        }

        public MovableRectangle( Factories.Factories factories, MovablePoint leftTop, MovablePoint rightTop, MovablePoint rightBottom, MovablePoint leftBottom, IRouteElement parent )
            : this( factories, parent )
        {
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.RightBottom = rightBottom;
            this.LeftBottom = leftBottom;
        }

        private MovableRectangle( Factories.Factories factories, IRouteElement parent )
        {
            this.Parent = parent;
            this._mouseHandler = factories.MouseHandlerFactory.Create(this);
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

        public override IMouseHandler MouseHandler
        {
            get { return this._mouseHandler; }
        }

        public override Vector2 Location
        {
            get { return this.LeftTop.Location; }
            // TDOO Fix it
            set
            {
                this.LeftBottom.SetLocation( value );
                this.Invalidate();
            }
        }

        public IRouteElement Parent { get; set; }

        public override void Translate( Matrix matrixTranslation )
        {
            this.Points.ForEach( s =>
                                    {
                                        s.Translate( matrixTranslation );
                                    } );
        }

        public override void TranslateWithoutNotification(Matrix translationMatrix)
        {
            this.Points.ForEach( s => s.TranslateWithoutNotification( translationMatrix ));
        }
    }
}