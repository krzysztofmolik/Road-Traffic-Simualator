using System;
using System.ComponentModel;
using Autofac;
using Common;
using Microsoft.Xna.Framework;
using NLog;
using RoadTrafficSimulator.Components.BuildMode;
using RoadTrafficSimulator.Components.SimulationMode;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Mouse;
using Game = Arcane.Xna.Presentation.Game;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using System.Linq;

namespace RoadTrafficSimulator
{
    public partial class XnaWindow : Game, INotifyPropertyChanged, IHandle<ChangedToSimulationMode>, IHandle<ChangedToBuildMode>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Autofac.IContainer _container;
        private KeyboardInputNotify _keybordInput;
        private MouseInformation _mouseInput;

        public XnaWindow( IServiceProvider service, Autofac.IContainer container, IEventAggregator eventAggregator )
            : base( service )
        {
            eventAggregator.Subscribe( this );
            this._container = container;
            this.InitializeComponent();
        }

        protected override void Initialize()
        {
            this._keybordInput = this._container.Resolve<KeyboardInputNotify>();
            this._mouseInput = this._container.Resolve<MouseInformation>();

            var buildModeComponent = this._container.Resolve<BuildModeMainComponent>();
            this.Components.Add( buildModeComponent );
        }

        public void RemoveComponent( IGameComponent component )
        {
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

        public void Handle( ChangedToSimulationMode message )
        {
            var buildComponent = this.Components.OfType<BuildModeMainComponent>().FirstOrDefault();
            if ( buildComponent == null ) { _logger.Warn( "Build component not present when switched to simulation mode" ); return; }
            var controls = buildComponent.GetAllBuildControls();
            var builder = new Builder();
            var simulationControls = builder.ConvertToSimulationMode( controls );
            var simulationMode = this._container.Resolve<SimulationModeMainComponent>();
            simulationControls.ForEach( simulationMode.AddRoadElement );

            this.Components.Remove( buildComponent );
            this.Components.Add( simulationMode );
        }

        public void Handle( ChangedToBuildMode message )
        {
            throw new NotImplementedException();
        }
    }
}
