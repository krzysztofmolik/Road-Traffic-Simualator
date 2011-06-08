﻿using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Infrastructure
{
    public static class Constans
    {
        public const float Epsilon = 0.001f;
        public const float DistanceArrowWidth = 0.10f;
        public const float DistanceArrowHeight = 0.02f;

        public const float RoadHeight = 0.1f;
        public const float Scale = 30.0f;
        public const float LaneWidth = 0.005f;

        public const float PointSize = 0.01f;
        public const float MeterToKmScale = 1000f;
        public const float FrameTimeMs = 33.3f;

        public static readonly Color RoadColor = new Color( 162, 162, 162 );
        public const float MsPerHour = 1000 * 3600;

        public const float CarMoveEpsilon = 0.00005f;

        public static float ToMeters( float value )
        {
            return value * Scale;
        }

        public static float ToVirtualUnit( float meters )
        {
            return meters / Scale;
        }

        public static float KmToVirtualUnit( float km )
        {
            return ToVirtualUnit( km * MeterToKmScale );
        }

        public static float VirtualUnitToKm( float virtualUnit )
        {
            return ToMeters( virtualUnit ) / MeterToKmScale;
        }
    }
}