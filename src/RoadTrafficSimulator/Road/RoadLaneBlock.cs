using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.VertexContainers;

namespace RoadTrafficSimulator.Road
{
    public class RoadLaneBlock : CompostControl<VertexPositionColor>, IRoadLaneBlock
    {
        private readonly IList<IControl> _roadBlocks; 
        private readonly RoadLaneBlockVertexContainer _roadLaneBlockVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly IControl _parent;
        private readonly Factories.Factories _factories;
        private MovablePoint _leftTopPoint;
        private MovablePoint _rightTop;
        private MovablePoint _leftBottom;
        private MovablePoint _rightBottom;

        private RoadLaneBlock()
        {
            this._roadLaneBlockVertexContainer = new RoadLaneBlockVertexContainer( this );
            this._mouseSupport = new CompositeControlMouseSupport( this );
            this._roadBlocks = new List<IControl>();
        }

        public RoadLaneBlock( Factories.Factories factories, IControl parent )
            : this()
        {
            this._parent = parent;
            this._factories = factories;
            this._leftTopPoint = new MovablePoint( factories, new Vector2( 1, 0 ), this );
            this._rightTop = new MovablePoint( factories, new Vector2( 1, 1 ), this );
            this._rightBottom = new MovablePoint( factories, new Vector2( 1, 0 ), this );
            this._leftBottom = new MovablePoint( factories, new Vector2( 0, 0 ), this );
            this.CreateEdges();
            this.AddToChildCollection();
        }

        public event EventHandler VectorChanged;

        #region Location properties

        public Vector2 LeftTopLocation
        {
            get { return this.LeftTopPoint.Location; }
        }

        public Vector2 LeftBottomLocation
        {
            get { return this.LeftBottomPoint.Location; }
        }

        public Vector2 RightTopLocation
        {
            get { return this.RightTopPoint.Location; }
        }

        public Vector2 RightBottomLocation
        {
            get { return this.RightBottomPoint.Location; }
        }

        public virtual Vector2 BeginLocation
        {
            get
            {
                var inCenter = this.RightTopLocation - this.LeftTopLocation;
                var half = inCenter * 0.5f;
                var translated = half + this.LeftTopLocation;

                return translated;
            }
        }

        public virtual Vector2 EndLocation
        {
            get
            {
                var inCenter = this.RightBottomLocation - this.LeftBottomLocation;
                var half = inCenter * 0.5f;
                var translated = half + this.LeftBottomLocation;

                return translated;
            }
        }

        #endregion Location properties

        #region MovablePoint propeties

        public MovablePoint LeftTopPoint
        {
            get
            {
                return this._leftTopPoint;
            }

            set
            {
                this._leftTopPoint = value;
                this.LeftEdge.EndPoint = value;
                this.TopEdge.StartPoint = value;
                this.Invalidate();
            }
        }

        public MovablePoint RightTopPoint
        {
            get
            {
                return this._rightTop;
            }

            set
            {
                this._rightTop = value;
                this.TopEdge.EndPoint = value;
                this.RightEdge.StartPoint = value;
                this.Invalidate();
            }
        }

        public MovablePoint RightBottomPoint
        {
            get
            {
                return this._rightBottom;
            }

            set
            {
                this._rightBottom = value;
                this.RightEdge.EndPoint = value;
                this.BottomEdge.StartPoint = value;
                this.Invalidate();
            }
        }

        public MovablePoint LeftBottomPoint
        {
            get
            {
                return this._leftBottom;
            }

            set
            {
                this._leftBottom = value;
                this.BottomEdge.EndPoint = value;
                this.LeftEdge.StartPoint = value;
                this.Invalidate();
            }
        }

        public EndRoadLaneEdge LeftEdge { get; private set; }

        public EndRoadLaneEdge RightEdge { get; private set; }

        public SideRoadLaneEdge TopEdge { get; private set; }

        public SideRoadLaneEdge BottomEdge { get; private set; }

        #endregion MovablePoint propeties

        public IList<IControl> RoadBlocks
        {
            get { return this._roadBlocks; }
        }

        public override IVertexContainer VertexContainer
        {
            get { return this._roadLaneBlockVertexContainer; }
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

        public IEnumerable<IControl> HitRoadBlock( Vector2 point )
        {
            var hitedBlocks = this.RoadBlocks.Where( t => t.IsHitted( point ) );

            return hitedBlocks;
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.LeftTopPoint.TranslateWithoutEvent( matrixTranslation );
            this.LeftTopPoint.Redraw();

            this.RightTopPoint.TranslateWithoutEvent( matrixTranslation );
            this.RightTopPoint.Redraw();

            this.RightBottomPoint.TranslateWithoutEvent( matrixTranslation );
            this.RightBottomPoint.Redraw();

            this.LeftBottomPoint.TranslateWithoutEvent( matrixTranslation );
            this.LeftBottomPoint.Redraw();

            // TODO WithoutEvent
            this.RoadBlocks.ForEach( b => b.Translate( matrixTranslation ) );
            this.Invalidate();
        }

        private void AddToChildCollection()
        {
            this.AddChild( this.LeftEdge );
            this.AddChild( this.TopEdge );
            this.AddChild( this.RightEdge );
            this.AddChild( this.BottomEdge );
            this._roadBlocks.ForEach( this.AddChild );
        }

        private void CreateEdges()
        {
            this.LeftEdge = new EndRoadLaneEdge( this._factories, this.LeftBottomPoint, this.LeftTopPoint, Constans.PointSize, this );
            this.TopEdge = new SideRoadLaneEdge( this._factories, this.LeftTopPoint, this.RightTopPoint, this );
            this.RightEdge = new EndRoadLaneEdge( this._factories, this.RightTopPoint, this.RightBottomPoint, Constans.PointSize, this );
            this.BottomEdge = new SideRoadLaneEdge( this._factories, this.RightBottomPoint, this.LeftBottomPoint, this );
        }

        public EndRoadLaneEdge OpositeEdge( EndRoadLaneEdge edge )
        {
            return this.LeftEdge == edge ? this.RightEdge : this.LeftEdge;
        }
    }
}