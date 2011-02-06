using System;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.VertexContainers;

namespace RoadTrafficSimulator.Road.Controls
{
    public class RoadJunctionBlock : CompostControl<VertexPositionColor>, IRoadJunctionBlock
    {
        private readonly RoadJunctionEdge[] _roadJunctionEdges = new RoadJunctionEdge[ EdgeType.Count ];
        private readonly MovablePoint[] _points = new MovablePoint[ Corners.Count ];
        private readonly IVertexContainer<VertexPositionColor> _specifiedVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly IControl _parent;

        public RoadJunctionBlock( Factories.Factories factories, Vector2 location, IControl parent )
        {
            this._parent = parent;
            const float halfRoadWidth = Constans.RoadHeight / 2;
            this._roadJunctionEdges = Enumerable.Range( 0, EdgeType.Count ).Select( s => new RoadJunctionEdge( factories, this ) ).ToArray();
            this._points = Enumerable.Range( 0, Corners.Count ).Select( s => new MovablePoint( factories, Vector2.Zero, this ) ).ToArray();
            var leftTop = new Vector2( location.X - halfRoadWidth, location.Y - halfRoadWidth );
            this.LeftTop = new MovablePoint( factories, leftTop, this );
            this.RightTop = new MovablePoint( factories, leftTop + new Vector2( Constans.RoadHeight, 0 ), this );
            this.RightBottom = new MovablePoint( factories, this.RightTop.Location + new Vector2( 0, Constans.RoadHeight ), this );
            this.LeftBottom = new MovablePoint( factories, this.RightBottom.Location + new Vector2( -Constans.RoadHeight, 0 ), this );

            this._points.ForEach( this.AddChild );
            this._roadJunctionEdges.ForEach( this.AddChild );

            this._specifiedVertexContainer = factories.VertexContainerFactory.Create( this );
            this._mouseSupport = new CompositeControlMouseSupport( this );
        }

        #region Poperties

        public Vector2 LeftTopLocation
        {
            get { return this.LeftTop.Location; }
        }

        public Vector2 RightTopLocation
        {
            get { return this.RightTop.Location; }
        }

        public Vector2 LeftBottomLocation
        {
            get { return this.LeftBottom.Location; }
        }

        public Vector2 RightBottomLocation
        {
            get { return this.RightBottom.Location; }
        }

        public MovablePoint[] Points
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
                this._points[ Corners.LeftBottom ] = value;
                this._roadJunctionEdges[ EdgeType.Left ].StartPoint = value;
                this._roadJunctionEdges[ EdgeType.Bottom ].EndPoint = value;
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
                this._points[ Corners.RightBottom ] = value;
                this._roadJunctionEdges[ EdgeType.Right ].EndPoint = value;
                this._roadJunctionEdges[ EdgeType.Bottom ].StartPoint = value;
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
                this._points[ Corners.RightTop ] = value;
                this._roadJunctionEdges[ EdgeType.Right ].StartPoint = value;
                this._roadJunctionEdges[ EdgeType.Top ].EndPoint = value;
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
                this._points[ Corners.LeftTop ] = value;
                this._roadJunctionEdges[ EdgeType.Top ].StartPoint = value;
                this._roadJunctionEdges[ EdgeType.Left ].EndPoint = value;
            }
        }

        public RoadJunctionEdge[] RoadJunctionEdges
        {
            get { return _roadJunctionEdges; }
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
            get { return this.LeftTopLocation; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }

        #endregion Properties

        public MovablePoint CornerHitTest( Vector2 point )
        {
            return this._points.FirstOrDefault( p => p.HitTest( point ) );
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.LeftTop.Translate( matrixTranslation );
            this.RightTop.Translate( matrixTranslation );
            this.RightBottom.Translate( matrixTranslation );
            this.LeftBottom.Translate( matrixTranslation );

            this.Children.ForEach( s => s.Invalidate() );
        }

        public void Normalize()
        {
        }
    }
}