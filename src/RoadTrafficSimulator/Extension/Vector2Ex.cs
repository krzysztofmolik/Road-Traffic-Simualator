using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using XnaVs10.Extension;

namespace XnaVs10.Extension
{
    public static class Vector2Ex
    {
        public static Vector2 LeftTop
        {
            get { return new Vector2( 0, 0 ); }
        }

        public static Vector2 LeftBottom
        {
            // TODO NIe jestem pewny
            get { return new Vector2( 0, 1 ); }
        }

        public static Vector2 RightTop
        {
            get { return new Vector2( 1, 0 ); }
        }

        public static Vector2 RightBottom
        {
            get { return new Vector2( 1, 1 ); }
        }

        public static Vector3 ToVector3( this Vector2 baseVector )
        {
            return new Vector3( baseVector.X, baseVector.Y, 0 );
        }

        public static bool IsValid( this Vector2 vector )
        {
            return !( Single.IsNaN( vector.X ) || Single.IsNaN( vector.Y ) );
        }

        public static float DistanceToLine( this Vector2 baseVector, Vector2 lineBegin, Vector2 lineEnd )
        {
            return PointDistanceFromLine( lineBegin, lineEnd, baseVector );
        }

        public static Matrix ToTranslationMatrix( this Vector2 vector2 )
        {
            return Matrix.CreateTranslation( vector2.ToVector3() );
        }

        public static float AngelBetween( this Vector2 baseVector, Vector2 secondVector )
        {
            baseVector.Normalize();
            secondVector.Normalize();

            return Vector2.Dot( baseVector, secondVector );
        }

        private static Vector2 closest_point_on_segment_AB_to_point_P( Vector2 A, Vector2 B, Vector2 P )
        {
            var u = B - A;

            var t = Vector2.Dot( P - A, u ) / Vector2.Dot( u, u );

            if ( t < 0 )
            {
                t = 0;
            }

            if ( t > 1 ) t = 1;
            {
                return A + u * t;
            }
        }

        private static float PointDistanceFromLine( Vector2 a, Vector2 b, Vector2 p )
        {
            var Pt = closest_point_on_segment_AB_to_point_P( a, b, p );

            return ( p - Pt ).Length();
        }
    }
}