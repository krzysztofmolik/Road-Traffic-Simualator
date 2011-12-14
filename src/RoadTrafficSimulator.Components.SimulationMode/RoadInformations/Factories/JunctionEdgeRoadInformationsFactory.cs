using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class JunctionEdgeRoadInformationsFactory : RoadInformationsFactoryBase<JunctionEdge>
    {
        private readonly Func<JunctionEdge, JunctionEdgeRoadInformation> _roadInformationFactory;

        public JunctionEdgeRoadInformationsFactory( Func<JunctionEdge, JunctionEdgeRoadInformation> roadInformationFactory )
        {
            this._roadInformationFactory = roadInformationFactory;
        }

        protected override IRoadInformation Create( JunctionEdge roadElemnet )
        {
            return this._roadInformationFactory( roadElemnet );
        }
    }
}