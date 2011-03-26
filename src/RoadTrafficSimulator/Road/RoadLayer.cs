using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road
{
    public class RoadLayer : CompostControl<VertexPositionColor>
    {
        private readonly RoadLayerVertexContainer _concretVertexContainer;
        private readonly Graphic _graphics;
        private readonly IMouseHandler _mouseSupport;
        private Vector2 _location = Vector2.Zero;

        public RoadLayer( Factories.Factories factories, Graphic graphics )
        {
            this._concretVertexContainer = new RoadLayerVertexContainer( this );
            this._graphics = graphics;
            this._mouseSupport = factories.MouseHandlerFactory.Create( this );
        }

        public override IVertexContainer VertexContainer
        {
            get { return this._concretVertexContainer; }
        }

        public override IMouseHandler MouseSupport
        {
            get { return this._mouseSupport; }
        }

        public override Vector2 Location
        {
            get { return this._location; }
        }

        public override IControl Parent
        {
            get { return null; }
        }

        public void Draw( GameTime timeSpan )
        {
            this._concretVertexContainer.Draw( this._graphics );

            this._graphics.VertexPositionalColorDrawer.Flush();
        }
        public override void Translate( Matrix matrixTranslation )
        {
            this._location = Vector2.Transform( this._location, matrixTranslation );
            this.Children.ForEach( s => s.Translate( matrixTranslation ) );
        }

        public override bool IsHitted( Vector2 location )
        {
            return false;
        }

    }
}