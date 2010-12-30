using System;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Road;
using XnaRoadTrafficConstructor.Road;

namespace Xna.Road
{
    public class SideRoadLine
    {
        private readonly IRoadLaneBlock[] _connectedRoads;

        public SideRoadLine( Vector2 leftPoint, Vector2 righPoint )
        {
            this.LeftCrosPoint = leftPoint;
            this.RightCrosPoint = righPoint;
            this._connectedRoads = new IRoadLaneBlock[( int ) ConnectedPlace.Count];
        }

        private enum ConnectedPlace
        {
            Front = 0,
            Left = 1,
            Right = 2,
            Count = 10
        }

        public Vector2 LeftCrosPoint { get; private set; }

        public Vector2 RightCrosPoint { get; private set; }

        public IRoadLaneBlock[] ConnectedRoads
        {
            get { return this._connectedRoads; }
        }

        public bool IsAnyRoadConnected
        {
            get { return this.ConnectedRoads.Any( t => t != null ); }
        }

        public void ConnectRoadLine( IRoadLaneBlock roadLaneBlock )
        {
            roadLaneBlock.NotNull();

            if ( this._connectedRoads.Any( t => t == roadLaneBlock ) )
            {
                return;
                //throw new ArgumentException( "Road is already connected" );
            }

            for ( int i = 0; i < this._connectedRoads.Length; ++i )
            {
                if ( this._connectedRoads[ i ] == null )
                {
                    this._connectedRoads[ i ] = roadLaneBlock;
                    return;
                }
            }

            throw new InvalidOperationException( "Can't connect more roads" );
        }
    }
}