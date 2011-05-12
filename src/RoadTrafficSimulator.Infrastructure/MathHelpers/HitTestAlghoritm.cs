using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Xna.Framework;

namespace XnaVs10.MathHelpers
{
    public class HitTestAlghoritm
    {
        public static bool HitTest( Vector2 hitPoint, params Vector2[] points )
        {
            Contract.Requires( points != null );
            var minx = points.Min( v => v.X );
            var minY = points.Min( v => v.Y );
            var vectors = points.ToArray();
            var hitPointOffset = hitPoint;
            if ( minx < 0 || minY < 0 )
            {
                var offsetX = Math.Abs( minx ) + 1;
                var offsetY = Math.Abs( minY ) + 1;
                for ( var i = 0; i < vectors.Length; ++i )
                {
                    vectors[ i ] = new Vector2( points[ i ].X + offsetX, points[ i ].Y + offsetY );
                }
                hitPointOffset = new Vector2( hitPoint.X + offsetX, hitPoint.Y + offsetY );
            }

            var corners = vectors.Length;
            var xCoard = vectors.Select( s => s.X ).ToArray();
            var yCoard = vectors.Select( s => s.Y ).ToArray();

            return PointInPolygon( corners, xCoard, yCoard, hitPointOffset );
        }

        public static bool PointInPolygon( int cornerSides, float[] xCord, float[] yCorad, Vector2 pointToTest )
        {
            var j = cornerSides - 1;
            var oddNodes = false;

            for ( var i = 0; i < cornerSides; i++ )
            {
                if ( yCorad[ i ] < pointToTest.Y &&
                     yCorad[ j ] >= pointToTest.Y ||
                     yCorad[ j ] < pointToTest.Y &&
                     yCorad[ i ] >= pointToTest.Y )
                {
                    if ( xCord[ i ] + ( pointToTest.Y - yCorad[ i ] ) / ( yCorad[ j ] - yCorad[ i ] ) * ( xCord[ j ] - xCord[ i ] ) < pointToTest.X )
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }

            return oddNodes;
        }
    }
}