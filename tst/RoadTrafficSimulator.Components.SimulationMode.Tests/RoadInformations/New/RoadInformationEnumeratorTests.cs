using Xunit;
using Xunit.Extensions;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadtrafficSimulator.Components.SimulationMode.TestsRoadInformations.New
{
    public class RoadInformationEnumeratorTests
    {
        [Fact]
        public void Should_iterate_over_all_elements_start_from_current()
        {
            var route = this.MakeRoute(
                                       this.CreateMock( 20.0f ),
                                       this.CreateMock( 20.0f ),
                                       this.CreateMock( 20.0f )
                                     );

            var enumerator = new RoadInformationEnumerator( route, 100.0f );
            Assert.Equal( enumerator.Count(), 3 );
        }

        [Fact]
        public void Should_iterate_until_max_distance_is_reached() // TODO Check word reached
        {
            var route = this.MakeRoute(
                                       this.CreateMock( 20.0f ),
                                       this.CreateMock( 20.0f ),
                                       this.CreateMock( 20.0f )
                                     );

            var enumerator = new RoadInformationEnumerator( route, 35.0f );

            Assert.Equal( enumerator.Count(), 2 );
        }

        private IRouteMark<IRoadInformation> MakeRoute( params IRoadInformation[] roadInformation )
        {
            var route = new Route.Route<IRoadInfomration>();
            roadInformation.ForEach( route.Add );
            return route;
        }

        private IRoadInformation CreateMock( float length )
        {
            var mock = new Mock<IRouteMark>();
            mock.Setup( m => m.Lenght( It.IsAny<IRoadElement>(), It.IsAny<IRoadElement>() ) ).Returns( length ); 
            return mock.Object;
        }
    }
}
