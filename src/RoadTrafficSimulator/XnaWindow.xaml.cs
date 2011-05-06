using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Utils;
using Xna;
using XnaVs10.Sprites;
using Game = Arcane.Xna.Presentation.Game;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;

namespace RoadTrafficSimulator
{
    public partial class XnaWindow : Game, INotifyPropertyChanged
    {
        private readonly Autofac.IContainer _serviceLocator;
        private KeyboardInputNotify _keybordInput;
        private MouseInputNotify _mouseInput;

        public XnaWindow( IServiceProvider service, Autofac.IContainer serviceLocator)
            : base( service )
        {
            this._serviceLocator = serviceLocator;
            this.InitializeComponent();
        }

        protected RoadComponent RoadComponent { get; private set; }

        protected override void Initialize()
        {
            this._keybordInput = this._serviceLocator.Resolve<KeyboardInputNotify>();
            this._mouseInput = this._serviceLocator.Resolve<MouseInputNotify>();

            this.RoadComponent = this._serviceLocator.Resolve<RoadComponent>();
            this.Components.Add( this.RoadComponent );
        }

        protected override void Update( GameTime gameTime )
        {
            this._mouseInput.Update( Mouse.GetState() );
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
