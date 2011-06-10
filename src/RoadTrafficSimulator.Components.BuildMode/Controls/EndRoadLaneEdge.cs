using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class LightVeretexContainer : VertexContainerBase<Light, VertexPositionTexture>
    {
        private readonly Texture2D _texture;
        private readonly TextureStyle _style;
        private static readonly float Width = Constans.ToVirtualUnit( 0.7f );
        private static readonly float Height = Constans.ToVirtualUnit( 1.5f );
        private static readonly int[] Indexes = new[] { 0, 3, 1, 1, 3, 2 };
        private Quadrangle _shape;

        public LightVeretexContainer( Light @object, Texture2D texture )
            : base( @object )
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
            graphic.VertexPositionalTextureDrawer.DrawIndexedTraingeList( this._texture, this.Vertex, Indexes);
        }
    }

    public class Light : SingleControl<VertexPositionTexture>
    {
        private Vector2 _location;
        private IContentManager _contentManager;
        private LightVeretexContainer _vertexContainer;
        private NotMovableMouseHandler _mouseHandler;

        public Light( Vector2 location, IContentManager contentManager )
        {
            this._location = location;
            this._contentManager = contentManager;

            this._vertexContainer = new LightVeretexContainer( this, this._contentManager.Load<Texture2D>( "Light" ) );
            this._mouseHandler = new NotMovableMouseHandler();
        }

        public override Vector2 Location { get { return this._location; } }

        public override IVertexContainer VertexContainer
        {
            get { return this._vertexContainer; }
        }

        public override IMouseHandler MouseHandler
        {
            get { return this._mouseHandler; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            throw new NotImplementedException();
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            throw new NotImplementedException();
        }

        public override IControl Parent { get; set; }
    }

    public class EndRoadLaneEdge : Edge
    {
        private RoadLaneBlock _parrent;
        private readonly IMouseHandler _notMovableMouseHandler;

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
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

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