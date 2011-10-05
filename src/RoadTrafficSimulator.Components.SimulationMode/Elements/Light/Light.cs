using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure.Controls;
using Routes = RoadTrafficSimulator.Components.SimulationMode.Builder.Routes;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements.Light
{
    public class Light : IRoadElement
    {
        private readonly LightBlock _lightBlock;
        private readonly LightStateMachine _stateMachine;
        private readonly LightDrawer _drawer;

        public Light( LightBlock lightBlock, Func<Light, IRoadInformation> conductorFactory )
        {
            this._lightBlock = lightBlock;
            this._stateMachine = new LightStateMachine( this );
            this._drawer = new LightDrawer( this );
            this.RoadInformation = conductorFactory( this );
        }

        public LightBlock LightBlock { get { return this._lightBlock; } }
        public IRoadElement Owner { get; set; }
        public IControl BuildControl { get { return this._lightBlock; } }

        public IRoadInformation RoadInformation { get; private set; }
        public LightState LightState { get { return this._stateMachine.State; } }
        public LightStateMachine StateMachine { get { return this._stateMachine; } }
        public IDrawer Drawer { get { return this._drawer; } }

        public Routes Routes
        {
            get { return Routes.Empty; }
        }
    }
}