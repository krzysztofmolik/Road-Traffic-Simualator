using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaVs10.Road;

namespace XnaRoadTrafficConstructor.Road
{
    public class Stored
    {
        private readonly IList<IRoadLaneBlock> _roadsLane = new List<IRoadLaneBlock>();
        private readonly IList<RoadLight> _lights = new List<RoadLight>();
        private readonly IList<StopLine> _stopLine = new List<StopLine>();
        private readonly IList<RoadLaneArrow> _roadLaneArrows = new List<RoadLaneArrow>();
        private readonly MessageBroker _messageBroker;
        private readonly IList<IRoadJunctionBlock> _roadJunctionBlocks = new List<IRoadJunctionBlock>();

        public Stored( MessageBroker messageBroker )
        {
            this._messageBroker = messageBroker.NotNull();

            this.Subscribe();
        }

        private void Subscribe()
        {

            this._messageBroker.RoadLaneCreated.Subscribe( args => this.AddRoadLane( args.Instance ) );

            this._messageBroker.RoadJunctionBlockCreated.Subscribe( args => this.AddRoadJunctionBlock( args.Instance ) );
        }

        //NOTE Fix it, return always new list, concurency problem.

        public IEnumerable<IRoadLaneBlock> RoadLanes
        {
            get { return this._roadsLane.ToArray(); }
        }

        public IEnumerable<RoadLight> RoadLights { get { return this._lights.ToArray(); } }

        public IEnumerable<StopLine> StopLines { get { return this._stopLine.ToArray(); } }

        public IEnumerable<RoadLaneArrow> RoadLaneArrows { get { return this._roadLaneArrows.ToArray(); } }

        public IEnumerable<IRoadLaneBlock> RoadLanesVertex { get { return this._roadsLane.ToArray(); } }

        public IEnumerable<IRoadJunctionBlock> RoadJunctionBlock
        {
            get { return this._roadJunctionBlocks.ToArray(); }
        }

        public IEnumerable<IRoadJunctionBlock> JunctionBlockVertex
        {
            get
            {
                return this._roadJunctionBlocks.ToArray();
            }
        }

        private void AddRoadLane( IRoadLaneBlock instance )
        {
            this._roadsLane.Add( instance );
        }

        private void AddRoadJunctionBlock( IRoadJunctionBlock block )
        {
            this._roadJunctionBlocks.Add( block );
        }
    }
}