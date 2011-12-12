using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Extensions;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;
using NLog;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RoadJunctionBlock : CompositControl<VertexPositionColor>, IRoadJunctionBlock
    {
        private readonly RoadJunctionEdge[] _roadJunctionEdges = new RoadJunctionEdge[ EdgeType.Count ];
        private readonly MovablePoint[] _points = new MovablePoint[ Corners.Count ];
        private readonly IVertexContainer<VertexPositionColor> _concretVertexContainer;
        private readonly IMouseHandler _mouseHandler;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public RoadJunctionBlock( Factories.Factories factories, Vector2 location, IControl parent )
        {
            this.Parent = parent;
            const float halfRoadWidth = Constans.RoadHeight / 2;
            this._roadJunctionEdges = Enumerable.Range( 0, EdgeType.Count ).Select( index => new RoadJunctionEdge( factories, this, index ) ).ToArray();
            this._points = new MovablePoint[ Corners.Count ];

            var leftTop = new Vector2( location.X - halfRoadWidth, location.Y + halfRoadWidth );
            this.LeftTop = new MovablePoint( factories, leftTop, this );
            this.RightTop = new MovablePoint( factories, leftTop + new Vector2( Constans.RoadHeight, 0 ), this );
            this.RightBottom = new MovablePoint( factories, this.RightTop.Location - new Vector2( 0, Constans.RoadHeight ), this );
            this.LeftBottom = new MovablePoint( factories, this.RightBottom.Location - new Vector2( Constans.RoadHeight, 0 ), this );

            this._points.ForEach( this.AddChild );
            this._roadJunctionEdges.ForEach( this.AddChild );
            this._roadJunctionEdges.ForEach( this.AddDefaultRoute );

            this._concretVertexContainer = factories.VertexContainerFactory.Create( this );
            this._mouseHandler = factories.MouseHandlerFactory.Create( this );
        }

        private void AddDefaultRoute( RoadJunctionEdge edge )
        {
            var opositeEdge = this.GetOpositeEdge( edge );
            var routeElements = new[] { new RouteElement( opositeEdge, PriorityType.None ) };
            edge.Routes.AddRoute( new Route( routeElements, 100.0f ) );
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
            get { return this._points[ Corners.LeftBottom ]; }
            set
            {
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
                this._points[ Corners.LeftTop ] = value;
                this._roadJunctionEdges[ EdgeType.Top ].StartPoint = value;
                this._roadJunctionEdges[ EdgeType.Left ].EndPoint = value;
            }
        }

        public RoadJunctionEdge[] RoadJunctionEdges
        {
            get { return _roadJunctionEdges; }
        }

        public override IVertexContainer VertexContainer
        {
            get { return this._concretVertexContainer; }
        }

        public override IMouseHandler MouseHandler
        {
            get { return this._mouseHandler; }
        }

        // TODO fix it
        public override Vector2 Location
        {
            get { return this.LeftTopLocation; }
            protected set
            {
                this.LeftTop.SetLocation( value );
                this.Invalidate();
            }
        }

        public override IControl Parent { get; set; }

        public RoadJunctionEdge LeftEdge { get { return this.RoadJunctionEdges[ EdgeType.Left ]; } }
        public RoadJunctionEdge TopEdge { get { return this.RoadJunctionEdges[ EdgeType.Top ]; } }
        public RoadJunctionEdge RightEdge { get { return this.RoadJunctionEdges[ EdgeType.Right ]; } }
        public RoadJunctionEdge BottomEdge { get { return this.RoadJunctionEdges[ EdgeType.Bottom ]; } }

        #endregion Properties

        public MovablePoint CornerHitTest( Vector2 point )
        {
            // TODO Change it
            var hittedControl = this._points.FirstOrDefault( p => p.IsHitted( point ) );
            if ( hittedControl != null )
            {
                return hittedControl.GetHittedControl( point ) as MovablePoint;
            }

            return null;
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.LeftTop.Translate( matrixTranslation );
            this.RightTop.Translate( matrixTranslation );
            this.RightBottom.Translate( matrixTranslation );
            this.LeftBottom.Translate( matrixTranslation );

            this.RoadJunctionEdges.ForEach( edge => edge.Invalidate() );
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            this.LeftTop.TranslateWithoutEvent( translationMatrix );
            this.RightTop.TranslateWithoutEvent( translationMatrix );
            this.RightBottom.TranslateWithoutEvent( translationMatrix );
            this.LeftBottom.TranslateWithoutEvent( translationMatrix );
        }

        protected override void OnInvalidate()
        {
            this.LeftTop.Invalidate();
            this.RightTop.Invalidate();
            this.RightBottom.Invalidate();
            this.LeftBottom.Invalidate();
            base.OnInvalidate();
        }

        public int GetEdgeType( Edge owner )
        {
            var edge = this.RoadJunctionEdges.Select( ( o, i ) => new { Item = o, Edge = i } ).FirstOrDefault( s => s.Item == owner );
            if ( edge == null )
            {
                _logger.Warn( "Edge not found" );
                return -1;
            }

            return edge.Edge;
        }
    }
}