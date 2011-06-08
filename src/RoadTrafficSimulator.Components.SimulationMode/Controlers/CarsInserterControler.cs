using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.Controlers
{
    public class CarsInserterControler : ControlersBase<CarsInserter>
    {
        private readonly ICarsFactory _carsFactory;

        public CarsInserterControler( ICarsFactory carsFactory )
        {
            Contract.Requires( carsFactory != null );
            this._carsFactory = carsFactory;
        }

        public override void Update( GameTime gameTime )
        {
            var now = DateTime.Now;
            this.Elements.ForEach( s => this.InserteCar( s, now ) );
            base.Update( gameTime );
        }

        private void InserteCar( CarsInserter carsInserter, DateTime now )
        {
            var nextInsertCarTime = carsInserter.LastTimeCarWasInseter + carsInserter.CarsInsertionInterval;
            if ( nextInsertCarTime < DateTime.Now )
            {
                carsInserter.LastTimeCarWasInseter = now;
                this._carsFactory.CreateCar( carsInserter );
            }
        }
    }
}