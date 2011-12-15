using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( LaneJunction ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class NonPriorityJunctionCondcutor : JunctionConductorBase
    {
        protected override PriorityInformation[] GetPriorityCarInfromation( Car car, IRouteMark<IConductor> route )
        {
            return new PriorityInformation[0];
        }
    }
}