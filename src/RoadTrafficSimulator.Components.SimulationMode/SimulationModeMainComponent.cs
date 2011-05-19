using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.SimulationMode.Controlers;
using DrawableGameComponent = RoadTrafficSimulator.Infrastructure.DrawableGameComponent;
using Common;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class SimulationModeMainComponent : DrawableGameComponent
    {
        private readonly IEnumerable<IControlers> _cosTams;
        private readonly List<IRoadElement> _roadElements = new List<IRoadElement>();

        public SimulationModeMainComponent( IGraphicsDeviceService graphicsDeviceService, IEnumerable<IControlers> cosTams )
            : base( graphicsDeviceService )
        {
            Contract.Requires( cosTams != null );
            this._cosTams = cosTams;
        }

        public void AddRoadElement( IRoadElement roadElement )
        {
            this._roadElements.Add( roadElement );
        }

        public override void Draw( GameTime time )
        {
            this._cosTams.ForEach( s => s.Draw( time ) );
        }

        public override void Update( GameTime time )
        {
            this._cosTams.ForEach( s => s.Update( time ) );
        }

        protected override void UnloadContent()
        {
        }

        protected override void LoadContent()
        {
        }
    }
}