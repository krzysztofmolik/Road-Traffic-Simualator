
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Road
{
    public class CarsInserterCreator
    {
        private IMouseInformation _mouseInformation;
        private bool _process;
        private ISubject<Vector2> _createCarsInserer = new Subject<Vector2>();

        public CarsInserterCreator( IMouseInformation mouseInformation )
        {
            this._mouseInformation = mouseInformation;
            this._mouseInformation.LeftButtonPressed.Where( s => this.Process ).Subscribe( s => this._createCarsInserer.OnNext( s.Location ) );
        }

        public bool Process
        {
            get { return this._process; }
        }

        public void Start()
        {
            if ( this._process ) { return; }
            this._process = true;
            this._mouseInformation.StartRecord();
        }
        public void Stop()
        {
            if ( this._process == false ) { return; }
            this._process = false;
            this._mouseInformation.StopRecord();
        }
    }
}