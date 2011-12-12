using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure.Controls;

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
            conductorFactory( this );
        }

        public LightBlock LightBlock { get { return this._lightBlock; } }
        public IRoadElement Owner { get; set; }
        public IControl BuildControl { get { return this._lightBlock; } }

        public LightState LightState { get { return this._stateMachine.State; } }
        public LightStateMachine StateMachine { get { return this._stateMachine; } }
        public IDrawer Drawer { get { return this._drawer; } }

        public IRoutes Routes
        {
            get { return StandardRoutes.Empty; }
        }
    }
}