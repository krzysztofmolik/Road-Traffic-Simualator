using System;
using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Utils;
using WinFormsGraphicsDevice;
using Xna;
using XnaRoadTrafficConstructor;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Sprites;
using XnaVs10.Utils;
using Game = Arcane.Xna.Presentation.Game;

namespace RoadTrafficSimulator
{
    public class XnaWindow : Game
    {
        private readonly IContainer _serviceLocator;
        private Camera3D _camera;
        private KeyboardInputNotify _keybordInput;
        private MouseInputNotify _mouseInput;

        public XnaWindow( IContainer service, IServiceProvider serviceProvider, MessageBroker messageBroker )
            : base( serviceProvider )
        {
            this._serviceLocator = service;
        }

        protected RoadComponent RoadComponent { get; private set; }

        protected Layer2D Layer2D { get; private set; }

        protected override void Initialize()
        {
            this._keybordInput = this._serviceLocator.Resolve<KeyboardInputNotify>();
            this._mouseInput = this._serviceLocator.Resolve<MouseInputNotify>();

            this.RoadComponent = this._serviceLocator.Resolve<RoadComponent>();
            this.Components.Add( this.RoadComponent );
            this.AddComponent( this.RoadComponent );

            this.Layer2D = this._serviceLocator.Resolve<Layer2D>();
            this.AddComponent( this.Layer2D );

            this._camera = this._serviceLocator.Resolve<Camera3D>();
            this._camera.Changed += ( sender, arg ) => this.Layer2D.UpdateGraphicDevice();
        }

        protected override void Draw( TimeSpan time )
        {
            GraphicsDevice.Clear( Color.CornflowerBlue );
        }

        protected override void UpdateMouse( TimeSpan gameTime )
        {
            this._mouseInput.Update( Mouse.GetState() );
        }

        protected override void UpdateKeyboard( TimeSpan time )
        {
            this._keybordInput.Update( Keyboard.GetState() );
        }
    }
}


