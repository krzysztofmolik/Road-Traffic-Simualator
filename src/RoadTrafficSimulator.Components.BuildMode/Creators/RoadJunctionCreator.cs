using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Creators
{
    public class RoadJunctionCreator
    {
        private readonly IMouseInformation _mouseInformation;
        private bool _shouldProcess;
        private readonly ISubject<Vector2> _juctionCreated = new Subject<Vector2>();

        public RoadJunctionCreator( IMouseInformation mouseInformation )
        {
            this._mouseInformation = mouseInformation.NotNull();

            this._mouseInformation.LeftButtonPressed.Where( s => this.Process )
                                                    .Subscribe( this.AddJunction );
        }

        public void AddRoadJunction()
        {
            this.Process = true;
        }

        private void AddJunction( XnaMouseState mouseState )
        {
            this._juctionCreated.OnNext( mouseState.Location );
        }

        protected bool Process
        {
            get
            {
                return _shouldProcess;
            }
            set
            {
                if ( this._shouldProcess == value )
                {
                    return;
                }

                this._shouldProcess = value;
                if ( this._shouldProcess )
                {
                    this._mouseInformation.StartRecord();
                }
                else
                {
                    this._mouseInformation.StopRecord();
                }
            }
        }

        public IObservable<Vector2> JunctionCreated { get { return this._juctionCreated; } }

        public void Begin()
        {
            this.Process = true;
        }

        public void End()
        {
            this.Process = false;
        }

    }
}