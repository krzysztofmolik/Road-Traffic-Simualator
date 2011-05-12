//  public domain function by Darel Rex Finley, 2006


//  Determines the intersection point of the line defined by points A and B with the

//  line defined by points C and D.

//

//  Returns YES if the intersection point was found, and stores that point in X,Y.

//  Returns No if there is no determinable intersection point, in which case X,Y will

//  be unmodified.


using System;

namespace Xna.MathHelpers
{
    public class GraphicUtils
    {
        public bool LineIntersection( double Ax, double Ay, double Bx, double By, double Cx, double Cy, double Dx, double Dy, out double X, out double Y )
        {
            X = default(double);
            Y = default(double);

            double distAB, theCos, theSin, newX, ABpos;

            //  Fail if either line is undefined.
            if ( Ax == Bx && Ay == By || Cx == Dx && Cy == Dy )
                return false;

            //  (1) Translate the system so that point A is on the origin.
            Bx -= Ax;
            By -= Ay;
            Cx -= Ax;
            Cy -= Ay;
            Dx -= Ax;
            Dy -= Ay;

            //  Discover the length of segment A-B.
            distAB = Math.Sqrt( Bx * Bx + By * By );

            //  (2) Rotate the system so that point B is on the positive X axis.
            theCos = Bx / distAB;
            theSin = By / distAB;
            newX = Cx * theCos + Cy * theSin;
            Cy = Cy * theCos - Cx * theSin;
            Cx = newX;
            newX = Dx * theCos + Dy * theSin;
            Dy = Dy * theCos - Dx * theSin;
            Dx = newX;

            //  Fail if the lines are parallel.
            if ( Cy == Dy )
            {
                return false;
            }

            //  (3) Discover the position of the intersection point along line A-B.
            ABpos = Dx + ( Cx - Dx ) * Dy / ( Dy - Cy );

            //  (4) Apply the discovered position to line A-B in the original coordinate system.
            X = Ax + ABpos * theCos;
            Y = Ay + ABpos * theSin;

            //  Success.
            return true;
        }
    }
}