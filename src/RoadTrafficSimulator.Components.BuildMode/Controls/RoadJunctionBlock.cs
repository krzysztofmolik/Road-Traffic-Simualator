using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;
using NLog;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RoadJunctionBlock : CompositControl<VertexPositionColor>, IRouteElement
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly InternalRoadJunctionEdge[] _junctionEdges;
        private readonly MovablePoint[] _points = new MovablePoint[ Corners.Count ];
        private readonly IVertexContainer<VertexPositionColor> _concretVertexContainer;
        private readonly IMouseHandler _mouseHandler;
        private readonly RoadJunctionBlockConnector _connector;

        public RoadJunctionBlock( Factories.Factories factories, Vector2 location )
        {
            const float halfRoadWidth = Constans.RoadHeight / 2;
            this._junctionEdges = Enumerable.Range( 0, EdgeType.Count ).Select( index => new InternalRoadJunctionEdge( factories, this, index ) ).ToArray();
            this._points = new MovablePoint[ Corners.Count ];
            this._connector = new RoadJunctionBlockConnector( this );

            var leftTop = new Vector2( location.X - halfRoadWidth, location.Y + halfRoadWidth );
            this.LeftTop = new MovablePoint( factories, leftTop, this );
            this.RightTop = new MovablePoint( factories, leftTop + new Vector2( Constans.RoadHeight, 0 ), this );
            this.RightBottom = new MovablePoint( factories, this.RightTop.Location - new Vector2( 0, Constans.RoadHeight ), this );
            this.LeftBottom = new MovablePoint( factories, this.RightBottom.Location - new Vector2( Constans.RoadHeight, 0 ), this );

            this._points.ForEach( this.AddChild );
            this._junctionEdges.ForEach( this.AddChild );

            this._concretVertexContainer = factories.VertexContainerFactory.Create( this );
            this._mouseHandler = factories.MouseHandlerFactory.Create( this );
        }

        #region Poperties

        public RoadJunctionBlockConnector Connector { get { return this._connector; } }

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
                this._junctionEdges[ EdgeType.Left ].StartPoint = value;
                this._junctionEdges[ EdgeType.Bottom ].EndPoint = value;
            }
        }

        public MovablePoint RightBottom
        {
            get { return this._points[ Corners.RightBottom ]; }
            set
            {
                this._points[ Corners.RightBottom ] = value;
                this._junctionEdges[ EdgeType.Right ].EndPoint = value;
                this._junctionEdges[ EdgeType.Bottom ].StartPoint = value;
            }
        }

        public MovablePoint RightTop
        {
            get { return this._points[ Corners.RightTop ]; }
            set
            {
                this._points[ Corners.RightTop ] = value;
                this._junctionEdges[ EdgeType.Right ].StartPoint = value;
                this._junctionEdges[ EdgeType.Top ].EndPoint = value;
            }
        }

        public MovablePoint LeftTop
        {
            get { return this._points[ Corners.LeftTop ]; }
            set
            {
                this._points[ Corners.LeftTop ] = value;
                this._junctionEdges[ EdgeType.Top ].StartPoint = value;
                this._junctionEdges[ EdgeType.Left ].EndPoint = value;
            }
        }

        public InternalRoadJunctionEdge[] JunctionEdges
        {
            get { return _junctionEdges; }
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
            set
            {
                this.LeftTop.SetLocation( value );
                this.Invalidate();
            }
        }

        public InternalRoadJunctionEdge LeftEdge { get { return this.JunctionEdges[ EdgeType.Left ]; } }
        public InternalRoadJunctionEdge TopEdge { get { return this.JunctionEdges[ EdgeType.Top ]; } }
        public InternalRoadJunctionEdge RightEdge { get { return this.JunctionEdges[ EdgeType.Right ]; } }
        public InternalRoadJunctionEdge BottomEdge { get { return this.JunctionEdges[ EdgeType.Bottom ]; } }

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

            this.JunctionEdges.ForEach( edge => edge.Invalidate() );
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            this.LeftTop.TranslateWithoutEvent( translationMatrix );
            this.RightTop.TranslateWithoutEvent( translationMatrix );
            this.RightBottom.TranslateWithoutEvent( translationMatrix );
            this.LeftBottom.TranslateWithoutEvent( translationMatrix );
        }

        public IEnumerable<IRouteElement> GetConnectedControls()
        {
            return this.Connector.Edges.Where( e => e != null ).Select( e => e.Edge.Parent );
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
            var edge = this.JunctionEdges.Select( ( o, i ) => new { Item = o, Edge = i } ).FirstOrDefault( s => s.Item == owner );
            if ( edge == null )
            {
                Logger.Warn( "Edge not found" );
                return -1;
            }

            return edge.Edge;
        }
    }
}