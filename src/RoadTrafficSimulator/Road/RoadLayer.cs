﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road
{
    public class RoadLayer : CompositControl<VertexPositionColor>
    {
        private readonly RoadLayerVertexContainer _concretVertexContainer;
        private readonly Graphic _graphics;
        private readonly IMouseHandler _mouseHandler;
        private Vector2 _location = Vector2.Zero;

        public RoadLayer( Factories.Factories factories, Graphic graphics )
        {
            this._concretVertexContainer = new RoadLayerVertexContainer( this );
            this._graphics = graphics;
            this._mouseHandler = factories.MouseHandlerFactory.Create( this );
        }

        public override IVertexContainer VertexContainer
        {
            get { return this._concretVertexContainer; }
        }

        public override IMouseHandler MouseHandler
        {
            get { return this._mouseHandler; }
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
        }

        public override void TranslateWithoutNotification(Matrix translationMatrix)
        {
        }

        public override bool IsHitted( Vector2 location )
        {
            return false;
        }

    }
}