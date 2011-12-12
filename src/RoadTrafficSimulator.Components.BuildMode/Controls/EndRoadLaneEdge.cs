using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class LightVeretexContainer : VertexContainerBase<LightBlock, VertexPositionTexture>
    {
        private readonly CachedTexture _texture;
        private static readonly float Width = Constans.ToVirtualUnit( 0.7f );
        private static readonly float Height = Constans.ToVirtualUnit( 1.5f );
        private Quadrangle _shape;

        public LightVeretexContainer( LightBlock @object, CachedTexture texture )
            : base( @object, Color.Transparent )
        {
            this._texture = texture;
        }

        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override VertexPositionTexture[] UpdateShapeAndCreateVertex()
        {
            this._shape = this.CreateQuatrangle();
            return new[]
                       {
                           new VertexPositionTexture(this._shape.LeftTop.ToVector3(), new Vector2(0, 1)),
                           new VertexPositionTexture(this._shape.RightTop.ToVector3(), new Vector2(1, 1)),
                           new VertexPositionTexture(this._shape.RightBottom.ToVector3(), new Vector2(1, 0)),
                           new VertexPositionTexture(this._shape.LeftBottom.ToVector3(), new Vector2(0, 0)),
                       };
        }

        private Quadrangle CreateQuatrangle()
        {
            return new Quadrangle(
                new Vector2( -Width / 2, -Height / 2 ) + this.Object.Location,
                new Vector2( Width / 2, -Height / 2 ) + this.Object.Location,
                new Vector2( Width / 2, Height / 2 ) + this.Object.Location,
                new Vector2( -Width / 2, Height / 2 ) + this.Object.Location );
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalTextureDrawer.DrawIndexedTraingeList( this._texture.Textrue, this.Vertex, this._shape.Indexes );
        }
    }


    public class LightConnector
    {
        private readonly LightBlock _owner;

        public LightConnector( LightBlock owner )
        {
            Contract.Requires( owner != null );
            this._owner = owner;
        }

        public Edge Owner { get; private set; }

        public void ConnectWith( Edge edge )
        {
            this.Owner = edge;
            edge.Translated.Subscribe( e => this._owner.SetLocation( e.Control.Location ) );
        }
    }

    public class LightBlock : SingleControl<VertexPositionTexture>
    {
        private Vector2 _location;
        private LightVeretexContainer _vertexContainer;
        private NotMovableMouseHandler _mouseHandler;
        private LightConnector _connector;
        private readonly CachedTexture _texture;

        public LightBlock( Vector2 location, CachedTexture texture )
        {
            this._location = location;
            this._texture = texture;

            this._vertexContainer = new LightVeretexContainer( this, this._texture );
            this._mouseHandler = new NotMovableMouseHandler();
            this._connector = new LightConnector( this );
        }

        public override Vector2 Location
        {
            get { return this._location; }
            protected set
            {
                this._location = value;
                this.Invalidate();
            }
        }

        public override IVertexContainer VertexContainer
        {
            get { return this._vertexContainer; }
        }

        public override IMouseHandler MouseHandler
        {
            get { return this._mouseHandler; }
        }

        public LightConnector Connector
        {
            get
            {
                return this._connector;
            }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            var location = this._location;
            this._location = Vector2.Transform( location, matrixTranslation );
            if ( location != this._location )
            {
                this.Invalidate();
            }
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            var location = this._location;
            this._location = Vector2.Transform( location, translationMatrix );
        }

        public override IControl Parent { get; set; }
    }

    public class EndRoadLaneEdge : Edge, IRoadElement
    {
        private RoadLaneBlock _parrent;
        private readonly IMouseHandler _notMovableMouseHandler;
        private readonly Routes _routes = new Routes();

        public EndRoadLaneEdge( Factories.Factories factories, RoadLaneBlock parent )
            : base( factories, Styles.NormalStyle )
        {
            this._parrent = parent;
            this._notMovableMouseHandler = factories.MouseHandlerFactory.CreateEmpty();
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public EndRoadLaneEdge( Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint, float width, RoadLaneBlock parent )
            : base( factories, startPoint, endPoint, Styles.NormalStyle )
        {
            this._parrent = parent;
            this._notMovableMouseHandler = factories.MouseHandlerFactory.CreateEmpty();
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public Routes Routes { get { return this._routes; } }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get { return this._parrent; }
        }

        public override IControl Parent
        {
            get { return this._parrent; }
            set
            {
                if ( ( value is RoadLaneBlock ) == false ) { throw new ArgumentException( "Only RoadLaneBlock is valid" ); }
                this._parrent = ( RoadLaneBlock ) value;
            }
        }

        public EndRoadLaneEdgeConnector Connector
        {
            get;
            private set;
        }

        public override Infrastructure.Mouse.IMouseHandler MouseHandler
        {
            get { return this._notMovableMouseHandler; }
        }

        public EndRoadLaneEdge GetOppositeEdge()
        {
            if ( this._parrent.LeftEdge == this )
            {
                return this._parrent.RightEdge;
            }

            // else
            return this._parrent.LeftEdge;
        }
    }
}