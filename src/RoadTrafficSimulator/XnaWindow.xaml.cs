using System;
using System.ComponentModel;
using Autofac;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Mouse;
using Game = Arcane.Xna.Presentation.Game;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using System.Linq;

namespace RoadTrafficSimulator
{
    public partial class XnaWindow : Game, INotifyPropertyChanged
    {
        private readonly Autofac.IContainer _container;
        private KeyboardInputNotify _keybordInput;
        private MouseInformation _mouseInput;

        public XnaWindow( IServiceProvider service, Autofac.IContainer container )
            : base( service )
        {
            this._container = container;
            this.InitializeComponent();
        }

        protected BuildModeMainComponent BuildModeMainComponent { get; private set; }

        protected override void Initialize()
        {
            this._keybordInput = this._container.Resolve<KeyboardInputNotify>();
            this._mouseInput = this._container.Resolve<MouseInformation>();

            this.BuildModeMainComponent = this._container.Resolve<BuildModeMainComponent>();
            this.Components.Add( this.BuildModeMainComponent );
        }

        public void RemoveComponent<T>() where T : class
        {
            var component = this.Components.OfType<T>().FirstOrDefault();
            if ( component != null )
            {
                this.Components.Remove( ( IGameComponent ) component );
            }
        }

        public void AddComponent( IGameComponent component )
        {
            this.Components.Add( component );
            component.Initialize();
        }

        protected override void Update( GameTime gameTime )
        {
            this._mouseInput.Update( gameTime );
            this._keybordInput.Update( Keyboard.GetState() );
            base.Update( gameTime );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged( string propertyName )
        {
            var handler = PropertyChanged;
            if ( handler != null ) handler( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}
