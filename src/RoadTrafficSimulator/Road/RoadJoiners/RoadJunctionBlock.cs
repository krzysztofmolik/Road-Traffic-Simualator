using System;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaRoadTrafficConstructor.VertexContainers;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public class RoadJunctionBlock : CompostControlBase<VertexPositionColor>, IRoadJunctionBlock
    {
        private readonly RoadJunctionEdge[] _roadJunctionEdges = new RoadJunctionEdge[ EdgeType.Count ];
        private readonly MovablePoint[] _points = new MovablePoint[ Corners.Count ];
        private readonly RoadJunctionBlockConnection _connectableObject;
        private readonly IVertexContainer<VertexPositionColor> _specifiedVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly ISelectionSupport _sellecteionSupport;
        private readonly RoadJunctionConnectionSupport _connectionSupport;

        public RoadJunctionBlock( Vector2 location, IControl parent )
            : base( parent )
        {
            const float halfRoadWidth = Constans.RoadHeight / 2;
            this._roadJunctionEdges = Enumerable.Range( 0, EdgeType.Count ).Select( s => new RoadJunctionEdge( this ) ).ToArray();
            this._points = Enumerable.Range( 0, Corners.Count ).Select( s => new MovablePoint( Vector2.Zero, this ) ).ToArray();
            var leftTop = new Vector2( location.X - halfRoadWidth, location.Y - halfRoadWidth );
            this.LeftTop = new MovablePoint( leftTop, this );
            this.RightTop = new MovablePoint( leftTop + new Vector2( Constans.RoadHeight, 0 ), this );
            this.RightBottom = new MovablePoint( this.RightTop.Location + new Vector2( 0, Constans.RoadHeight ), this );
            this.LeftBottom = new MovablePoint( this.RightBottom.Location + new Vector2( -Constans.RoadHeight, 0 ), this );

            this._connectableObject = new RoadJunctionBlockConnection( this );

            this._points.ForEach( this.AddChild );
            this._roadJunctionEdges.ForEach( this.AddChild );

            this._specifiedVertexContainer = new RoadJunctionBlockVertexContainer( this );
            this._mouseSupport = new CompositeControlMouseSupport( this );
            this._sellecteionSupport = new DefaultCompositeControlSelectionSupport( this );
            this._connectionSupport = new RoadJunctionConnectionSupport( this );
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

        public RoadJunctionBlockConnection ConnectableObject
        {
            get { return this._connectableObject; }
        }

        public MovablePoint LeftBottom
        {
            get { return this._points[ Corners.LeftBottom ]; }
            set
            {
                value.AddParent( this );
                this._points[ Corners.LeftBottom ] = value;
                this._roadJunctionEdges[ EdgeType.Left ].StartPoint = value;
                this._roadJunctionEdges[ EdgeType.Bottom ].EndPoint = value;
            }
        }

        public MovablePoint RightBottom
        {
            get { return this._points[ Corners.RightBottom ]; }
            set
            {
                value.AddParent( this );
                this._points[ Corners.RightBottom ] = value;
                this._roadJunctionEdges[ EdgeType.Right ].EndPoint = value;
                this._roadJunctionEdges[ EdgeType.Bottom ].StartPoint = value;
            }
        }

        public MovablePoint RightTop
        {
            get { return this._points[ Corners.RightTop ]; }
            set
            {
                value.AddParent( this );
                this._points[ Corners.RightTop ] = value;
                this._roadJunctionEdges[ EdgeType.Right ].StartPoint = value;
                this._roadJunctionEdges[ EdgeType.Top ].EndPoint = value;
            }
        }

        public MovablePoint LeftTop
        {
            get { return this._points[ Corners.LeftTop ]; }

            set
            {
                value.AddParent( this );
                this._points[ Corners.LeftTop ] = value;
                this._roadJunctionEdges[ EdgeType.Top ].StartPoint = value;
                this._roadJunctionEdges[ EdgeType.Left ].EndPoint = value;
            }
        }

        #endregion Properties

        public MovablePoint CornerHitTest( Vector2 point )
        {
            return this._points.FirstOrDefault( p => p.HitTest( point ) );
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

        public override IConnectionCompositeSupport ConnectionSupport
        {
            get { return this._connectionSupport; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.LeftTop.Translate( matrixTranslation );
            this.RightTop.Translate( matrixTranslation );
            this.RightBottom.Translate( matrixTranslation );
            this.LeftBottom.Translate( matrixTranslation );
        }

        public override Vector2 Location
        {
            get { return this.LeftTopLocation; }
        }

        public override ISelectionSupport SelectionSupport
        {
            get { return this._sellecteionSupport; }
        }
    }
}