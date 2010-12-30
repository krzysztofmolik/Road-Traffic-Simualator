using Common;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaVs10.Road;

namespace RoadTrafficSimulator.Road
{
    public class RoadJunctionBlockConnection
    {
        private readonly IRoadJunctionBlock _block;

        public RoadJunctionBlockConnection( IRoadJunctionBlock block )
        {
            this.RoadConnector = new ConetableBase<IRoadJunctionBlock>();
            this._block = block.NotNull();
            this.RoadConnector = new ConetableBase<IRoadJunctionBlock>();
        }

        public ConetableBase<IRoadJunctionBlock> RoadConnector { get; private set; }

        public void Connect( IRoadJunctionBlock roadJunctionBlock )
        {
            //Get connector
            //Znajdz jakie punkty sa do polaczenia
            //Polacz
            //Poinformuj

            roadJunctionBlock.LeftBottom = this._block.RightBottom;
            roadJunctionBlock.LeftTop = this._block.RightTop;
            this.RoadConnector.Connect( roadJunctionBlock.ConnectableObject.RoadConnector );
        }
    }
}