using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class LightBlock : SingleControl<VertexPositionTexture>
    {
        private Vector2 _location;
        private readonly LightVeretexContainer _vertexContainer;
        private readonly NotMovableMouseHandler _mouseHandler;
        private readonly LightConnector _connector;
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
            set
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
    }
}