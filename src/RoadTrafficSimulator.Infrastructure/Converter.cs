namespace RoadTrafficSimulator.Infrastructure
{
    public static class UnitConverter
    {
        public static float FromMeter( float meters )
        {
            return meters / Constans.Scale;
        }

        public static float ToMeter( float virtualUnit )
        {
            return virtualUnit * Constans.Scale;
        }

        public static float FromKm( float km )
        {
            return FromMeter( km * Constans.MetersPerKm );
        }

        public static float ToKm( float virtualUnit )
        {
            return ToMeter( virtualUnit / Constans.MetersPerKm );
        }

        public static float FromMeterPerSecond( float metersPerSecond )
        {
            var unitPerHour = FromMeter( metersPerSecond );

            return unitPerHour / Constans.MsPerSecond;
        }

        public static float FromKmPerHour( float kmPerHour )
        {
            var km = FromKm( kmPerHour );
            return km / Constans.MsPerHour;
        }

        public static float ToKmPerHour( float virtualUnitPerMs )
        {
            var km = ToKm( virtualUnitPerMs );
            return km * Constans.MsPerHour;
        }

        public static float FromSecond( float seconds )
        {
            return seconds * Constans.MsPerSecond;
        }
    }
}