using System;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Components.BuildMode.Creators
{
    public class RoadLaneCommandController : Command
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly VisitAllChildren _visitator;
        private readonly RoadLaneCreator _roadLaneCreator;
        private bool _isFirst;
        private readonly IControl _owner;

        public RoadLaneCommandController( IMouseInformation mouseInformation, RoadLayer ownr, RoadLaneCreator roadLaneCreator )
        {
            this._mouseInformation = mouseInformation;
            this._roadLaneCreator = roadLaneCreator;
            this._mouseInformation.LeftButtonPressed.Subscribe( this.MousePressed );
            this._owner = ownr;

            this._visitator = new VisitAllChildren( this._owner );
        }

        // This is stupid, should be changed
        public void SetOwner( ICompositeControl owner )
        {
            this._roadLaneCreator.SetOwner( owner );
        }

        public Type CreatedType
        {
            get { return typeof( RoadLaneBlock ); }
        }

        public void Start()
        {
            this._isFirst = true;
            this._mouseInformation.StartRecord();
        }

        public void Stop()
        {
            this._mouseInformation.StopRecord();

            // TODO Finish road lane at some control
        }

        private void MousePressed( XnaMouseState mouseState )
        {
            var edge = this.GetRoadJuctionEdge( mouseState.Location );
            if ( this.IsAppropiate( edge ) == false ) { return; }

            if ( this._isFirst )
            {
                this.ProcessFirstControl( edge );
            }
            else
            {
                this.ProcessControl( edge, mouseState.Location );
            }
        }

        private bool IsAppropiate( IControl edge )
        {
            // TODO: Fix it, this should be resolved in more appropiate way
            return edge == null || edge is RoadJunctionEdge || edge is CarsInserter || edge is CarsRemover;
        }

        private void ProcessControl( IControl edge, Vector2 location )
        {
            if ( edge == null )
            {
                this._roadLaneCreator.CreateBlockTo( location );
            }
            else
            {
                this._roadLaneCreator.EndIn( edge );
                this.StartFromBegining();
            }
        }

        private void StartFromBegining()
        {
            this._isFirst = true;
        }

        private void ProcessFirstControl( IControl control )
        {
            if ( control == null ) { return; }

            this._isFirst = false;
            this._roadLaneCreator.StartFrom( control );
        }

        private IControl GetRoadJuctionEdge( Vector2 location )
        {
            return this._visitator.Where( c => c.IsHitted( location ) ).FirstOrDefault();
        }
    }
}