using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RoadLayer : CompositControl<VertexPositionColor>, IHandle<NewControlCreated>
    {
        private readonly RoadLayerVertexContainer _concretVertexContainer;
        private readonly Graphic _graphics;
        private readonly IMouseHandler _mouseHandler;
        private Vector2 _location = Vector2.Zero;
        private readonly Queue<Action> _actions = new Queue<Action>();

        public RoadLayer( Factories.Factories factories, Graphic graphics, IEventAggregator eventAggregator )
        {
            this._concretVertexContainer = new RoadLayerVertexContainer( this );
            this._graphics = graphics;
            this._mouseHandler = factories.MouseHandlerFactory.Create( this );
            eventAggregator.Subscribe( this );
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
            set
            {
                this._location = value;
                this.Invalidate();
            }
        }

        public override void AddChild( IControl control )
        {
            this._actions.Enqueue( () => base.AddChild( control ) );
        }

        public void Draw( GameTime timeSpan )
        {
            this._concretVertexContainer.Draw( this._graphics );
            this._graphics.VertexPositionalColorDrawer.Flush();
            this._graphics.VertexPositionalTextureDrawer.Flush();
            this.RunQueueActions();
        }

        private void RunQueueActions()
        {
            while ( this._actions.Count > 0 )
            {
                var action = this._actions.Dequeue();
                action();
            }
        }

        public override void Translate( Matrix matrixTranslation )
        {
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
        }

        public override bool IsHitted( Vector2 location )
        {
            return false;
        }

        public void Handle( NewControlCreated message )
        {
            this.AddChild( message.Control );
        }
    }
}