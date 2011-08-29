using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class NextAvailavlePointToStopInfoIterator
    {
        public NextAvailablePointToStopInfo Get( IRouteMark route, float maxLenght, Car car )
        {
            var result = new NextAvailablePointToStopInfo();
            var currentLocation = route.Current.Condutor.GetCarDistanceToEnd( car );
            result.AddRange( 0.0f, currentLocation, route.Current.Condutor.CanStop( route.GetPrevious(), route.GetNext() ) );
            while ( route.MoveNext() && currentLocation < maxLenght )
            {
                var lenght = route.Current.Condutor.Lenght( route.GetPrevious(), route.GetNext() );
                result.AddRange( currentLocation, currentLocation + lenght, route.Current.Condutor.CanStop( route.GetNext(), route.GetPrevious() ) );
                currentLocation += lenght;
            }

            return result;
        }
    }
}