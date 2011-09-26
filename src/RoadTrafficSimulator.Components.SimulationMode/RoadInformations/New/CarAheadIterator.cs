using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.New
{
    public class CarAheadIterator
    {
        public class Information
        {
            public Information( float distance, float speed )
            {
                this.Distance = distance;
                this.Speed = speed;
            }

            public float Distance { get; set; }
            public float Speed { get; private set; }
        }

        public Information Get( Car car )
        {
            var roadElements = car.RoadElements.Clone();
            var carAhead = car.RoadElements.Current.RoadInformation.GetCarAheadDistance();
            if ( carAhead != null ) 
            { return new Information( Vector2.Distance( car.Location, carAhead.Location ), carAhead.Velocity ); }

            var length = car.RoadElements.Current.RoadInformation.GetCarDistanceToEnd( car );
            while ( roadElements.MoveNext() )
            {
                carAhead = roadElements.Current.RoadInformation.GetCarAheadDistance();
                if ( carAhead != null )
                {
                    length += roadElements.Current.RoadInformation.GetCarDistanceToBegin( carAhead );
                    break;
                }

                length += roadElements.Current.RoadInformation.Lenght( roadElements.GetPrevious(), roadElements.GetNext() );
            }

            return new Information( length, carAhead != null ? carAhead.Velocity : float.MaxValue );
        }
    }
}
