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
        private readonly ISelectionSupport _selectionSupport;
        private readonly IControl _parent;
        private MovablePoint _leftTopPoint;
        private MovablePoint _rightTop;
        private MovablePoint _leftBottom;
        private MovablePoint _rightBottom;

        public RoadLaneBlock( IControl parent )
        {
            this._parent = parent;
            this._roadLaneBlockVertexContainer = new RoadLaneBlockVertexContainer( this );
            this._mouseSupport = new CompositeControlMouseSupport( this );
            this._selectionSupport = new DefaultCompositeControlSelectionSupport( this );

            this._leftTopPoint = new MovablePoint( new Vector2( 1, 0 ), this );
            this._rightTop = new MovablePoint( new Vector2( 1, 1 ), this );
            this._rightBottom = new MovablePoint( new Vector2( 1, 0 ), this );
            this._leftBottom = new MovablePoint( new Vector2( 0, 0 ), this );
            this.CreateEdges();
            this._roadBlocks = this.CreateRoadBlock();
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
                this.ChangedSubject.OnNext( new Unit() );
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
                this.ChangedSubject.OnNext( new Unit() );
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
                this.ChangedSubject.OnNext( new Unit() );
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
                this.ChangedSubject.OnNext( new Unit() );
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

        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
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

        public override ISelectionSupport SelectionSupport
        {
            get { return this._selectionSupport; }
        }

        public IEnumerable<IControl> HitRoadBlock( Vector2 point )
        {
            var hitedBlocks = this.RoadBlocks.Where( t => t.HitTest( point ) );

            return hitedBlocks;
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.LeftTopPoint.Translate( matrixTranslation );
            this.RightTopPoint.Translate( matrixTranslation );
            this.RightBottomPoint.Translate( matrixTranslation );
            this.LeftBottomPoint.Translate( matrixTranslation );

            this.RoadBlocks.ForEach( b => b.Translate( matrixTranslation ) );
            this.ChangedSubject.OnNext( new Unit() );
        }

        protected void RaiseVectorChanged()
        {
            if ( this.VectorChanged != null )
            {
                this.VectorChanged( this, EventArgs.Empty );
            }
        }

        private List<IControl> CreateRoadBlock()
        {
            return new List<IControl>();
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
            this.LeftEdge = new EndRoadLaneEdge( this.LeftBottomPoint, this.LeftTopPoint, Constans.PointSize, this );
            this.TopEdge = new SideRoadLaneEdge( this.LeftTopPoint, this.RightTopPoint, Constans.PointSize, this );
            this.RightEdge = new EndRoadLaneEdge( this.RightTopPoint, this.RightBottomPoint, Constans.PointSize, this );
            this.BottomEdge = new SideRoadLaneEdge( this.RightBottomPoint, this.LeftBottomPoint, Constans.PointSize, this );
        }
    }
}