using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Road.Controls;
using XnaVs10.Utils;

namespace XnaVs10.MathHelpers
{
    public static class MyMathHelper
    {
        public static float AngelBetwenVectors( Vector2 first, Vector2 second )
        {
            var fistNormalized = Vector2.Normalize( first );
            var secondNormalized = Vector2.Normalize( second );

            return ( float ) Math.Acos( Vector2.Dot( fistNormalized, secondNormalized ) );
        }
        public static Vector2 CreatePerpendicularVector( Vector2 baseVector, float length )
        {
            var perpendicualrVector = new Vector2( baseVector.Y, -baseVector.X );
            perpendicualrVector.Normalize();
            return perpendicualrVector * length;
        }

        public static Tuple<Vector2, Vector2> CreatePerpendicualrLine( Line line, float length )
        {
            return CreatePerpendicualrLine( line.Begin, line.End, length );
        }

        public static Tuple<Vector2, Vector2> CreatePerpendicualrLine( Vector2 startPoint, Vector2 endPoint, float length )
        {
            var attachedInCenter = startPoint - endPoint;
            var papendicualrHalfLength = CreatePerpendicularVector( attachedInCenter, length / 2 );
            var difrentDirection = papendicualrHalfLength * -1.0f;

            return Tuple.Create( papendicualrHalfLength + startPoint, difrentDirection + startPoint );
        }

        public static Vector2 LineIntersectionMethod( Line left, Line right )
        {
            return LineIntersectionMethod( Tuple.Create( left.Begin, left.End ),
                                           Tuple.Create( right.Begin, right.End ) );
        }

        public static Vector2 LineIntersectionMethod( Tuple<Vector2, Vector2> left, Tuple<Vector2, Vector2> right )
        {
            return LineIntersectionMethod( left.Item1, left.Item2, right.Item1, right.Item2 );
        }

        public static Vector2 LineIntersectionMethod( Vector2 leftBegin, Vector2 leftEnd, Vector2 rightBegin, Vector2 rightEnd )
        {
            var firstBegin = leftBegin;
            var firstEnd = leftEnd;
            var secondBegin = rightBegin;
            var secondEnd = rightEnd;
            double distAB, theCos, theSin, newX, ABpos;

            //  Fail if either line is undefined.
            if ( firstBegin.X == firstEnd.X && firstBegin.Y == firstEnd.Y || secondBegin.X == secondEnd.X && secondBegin.Y == secondEnd.Y )
            {
                throw new ArgumentException( "Linie is undefined" );
            }

            //  (1) Translate the system so that point A is on the origin.
            firstEnd.X -= firstBegin.X;
            firstEnd.Y -= firstBegin.Y;
            secondBegin.X -= firstBegin.X;
            secondBegin.Y -= firstBegin.Y;
            secondEnd.X -= firstBegin.X;
            secondEnd.Y -= firstBegin.Y;

            //  Discover the length of segment A-B.
            distAB = Math.Sqrt( firstEnd.X * firstEnd.X + firstEnd.Y * firstEnd.Y );

            //  (2) Rotate the system so that point B is on the positive X axis.
            theCos = firstEnd.X / distAB;
            theSin = firstEnd.Y / distAB;
            newX = secondBegin.X * theCos + secondBegin.Y * theSin;
            secondBegin.Y = ( float ) ( secondBegin.Y * theCos - secondBegin.X * theSin );
            secondBegin.X = ( float ) newX;
            newX = secondEnd.X * theCos + secondEnd.Y * theSin;
            secondEnd.Y = ( float ) ( secondEnd.Y * theCos - secondEnd.X * theSin );
            secondEnd.X = ( float ) newX;

            //  Fail if the lines are parallel.
            if ( secondBegin.Y == secondEnd.Y )
            {
                var beginToBegin = Vector2.Distance( leftBegin, rightBegin );
                var beginToEnd = Vector2.Distance( leftBegin, rightEnd );

                var endToBegin = Vector2.Distance( leftEnd, rightBegin );
                var endToEnd = Vector2.Distance( leftEnd, rightEnd );


                // TODO Naprawic !!
                if ( beginToEnd < endToEnd )
                {
                    return leftBegin;
                }

                // else
                return leftEnd;
            }

            //  (3) Discover the position of the intersection point along line A-B.
            ABpos = secondEnd.X + ( secondBegin.X - secondEnd.X ) * secondEnd.Y / ( secondEnd.Y - secondBegin.Y );

            //  (4) Apply the discovered position to line A-B in the original coordinate system.
            var x = firstBegin.X + ABpos * theCos;
            var y = firstBegin.Y + ABpos * theSin;
            return new Vector2( ( float ) x, ( float ) y );
        }

        public static Tuple<Vector2, Vector2> CreateTShape( Vector2 begin, Vector2 end, float TWidth )
        {
            // stworz linie prostopadla o podanej dlugosci
            var parpendicualrLine = MyMathHelper.CreatePerpendicualrLine( begin, end, TWidth );

            return parpendicualrLine;
        }

        public static Vector2 GetNewLocationWithMinmalDistance( Vector2 location, Vector2 vector2, float roadHeight )
        {
            var inCenter = vector2 - location;
            inCenter.Normalize();
            var correctLenght = inCenter * roadHeight;
            return correctLenght + location;
        }

        public static Quadrangle CreateQuadrangleFromLocation( Vector2 first, Vector2 second, float height )
        {
            var leftEdge = CreatePerpendicualrLine( new Line( first, second ), height );
            var rightEdge = CreatePerpendicualrLine( new Line( second, first ), height );
            return new Quadrangle(leftEdge.Item2, rightEdge.Item1, rightEdge.Item2, leftEdge.Item1);
        }
    }
}