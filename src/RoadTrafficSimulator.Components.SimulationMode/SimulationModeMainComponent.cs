using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.SimulationMode.Controlers;
using DrawableGameComponent = RoadTrafficSimulator.Infrastructure.DrawableGameComponent;
using Common;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class SimulationModeMainComponent : DrawableGameComponent
    {
        private readonly IEnumerable<IControlers> _controlers;
        private readonly List<IRoadElement> _roadElements = new List<IRoadElement>();

        public SimulationModeMainComponent( IGraphicsDeviceService graphicsDeviceService, IEnumerable<IControlers> controlers, IEventAggregator eventAggregator)
            : base( graphicsDeviceService, eventAggregator )
        {
            Contract.Requires( controlers != null );
            this._controlers = controlers.OrderBy( c => c.Order).ToArray();
        }

        public void AddRoadElement( IRoadElement roadElement )
        {
            this._roadElements.Add( roadElement );
            this._controlers.ForEach( s => s.AddControl( roadElement ) );
        }

        public override void Draw( GameTime time )
        {
            this._controlers.ForEach( s => s.Draw( time ) );
        }

        public override void Update( GameTime time )
        {
            this._controlers.ForEach( s => s.Update( time ) );
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            Console.WriteLine("UnloadContent");
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}