namespace Common.Xna

open Microsoft.Xna.Framework;
open Microsoft.Xna.Framework.Graphics;

    [<AutoOpen>]
    module Math =
        let toTuple ( vector:Vector2 ) = ( vector.X, vector.Y )

        let CreatePerpendicularVector ( vector: Vector2 )  length =
            let perpendicualrVector = new Vector2( vector.Y, -vector.X );
            perpendicualrVector.Normalize();
            perpendicualrVector * length;

        let parpendicularLine startPoint endPoint length  = 
            let center = startPoint - endPoint
            let papendicualrHalfLength = CreatePerpendicularVector center ( length / 2.0f )
            let difrentDirection = papendicualrHalfLength * -1.0f

            let startVector =  papendicualrHalfLength + startPoint
            let endVector =  difrentDirection + startPoint 

            ( startVector, endVector )

        let parpendicularLineTuple( startPoint, endPoint, length ) = 
            if( startPoint.Equals( endPoint ) ) 
                then raise (System.ArgumentException("start point and end point are equal" ) ) else
            let result = parpendicularLine startPoint endPoint length
            result

//        let lineIntersectionMethod( (leftBegin:Vector2), (leftEnd:Vector2), (rightBegin:Vector2), (rightEnd:Vector2) ) =
//            let m1 = ( leftEnd.Y - leftBegin.Y ) / ( leftEnd.X - leftBegin.X );
//            let m2 = ( rightEnd.Y - rightBegin.Y ) / ( rightEnd.X- rightBegin.X );
//
//            let b1 = leftBegin.Y - ( m1 * leftBegin.X );
//            let b2 = rightBegin.Y - ( m2 * rightBegin.X );
//
//            let x_intersect = ( b2 - b1 ) / ( m1 - m2 );
//            let y_intersect = ( m1 * x_intersect ) + b1;
//            Vector2( x_intersect, y_intersect )

        let lineIntersectionMethod( (leftBegin:Vector2), (leftEnd:Vector2), (rightBegin:Vector2), (rightEnd:Vector2) ) =
            let sqrt value =  
                let result = System.Math.Sqrt( float( value ) )
                float32( result )

            let firstBegin = leftBegin;
            let firstEnd = leftEnd;
            let secondBegin = rightBegin;
            let secondEnd = rightEnd;

            //  Fail if either line is undefined.
            if ( firstBegin.X = firstEnd.X && firstBegin.Y = firstEnd.Y || secondBegin.X = secondEnd.X && secondBegin.Y = secondEnd.Y ) then
                raise( System.ArgumentException( "Linie is undefined" ) )

            //  (1) Translate the system so that point A is on the origin.
            let firstEndXOrgin = firstEnd.X - firstBegin.X;
            let firstEndYOrgin = firstEnd.Y - firstBegin.Y;
            let secondBeginXOrgin = secondBegin.X - firstBegin.X;
            let secondBeginYOrgin = secondBegin.Y - firstBegin.Y;
            let secondEndXOrgin = secondEnd.X - firstBegin.X;
            let secondEndYOrgin = secondEnd.Y - firstBegin.Y;

            //  Discover the length of segment A-B.
            let distAB = sqrt( firstEndXOrgin * firstEndXOrgin + firstEndYOrgin * firstEndYOrgin );

            //  (2) Rotate the system so that point B is on the positive X axis.
            let theCos = firstEndXOrgin / distAB;
            let theSin = firstEndYOrgin / distAB;
            let newX = secondBeginXOrgin * theCos + secondBeginYOrgin * theSin;
            let secondBeginYRotated = ( float32 ) ( secondBeginYOrgin * theCos - secondBeginXOrgin * theSin );
            let secondBeginXRotated = ( float32 ) newX;
            let newEndX = secondEndXOrgin * theCos + secondEndYOrgin * theSin;
            let secondEndYRotated = ( float32 ) ( secondEndYOrgin * theCos - secondEndXOrgin * theSin );
            let secondEndXRotated = ( float32 ) newEndX;

            //  Fail if the lines are parallel.
            if ( secondBeginYRotated = secondEndYRotated ) then
                let beginToBegin = Vector2.Distance( leftBegin, rightBegin )
                let beginToEnd = Vector2.Distance( leftBegin, rightEnd )

                let endToBegin = Vector2.Distance( leftEnd, rightBegin );
                let endToEnd = Vector2.Distance( leftEnd, rightEnd );


                // TODO Naprawic !!
                if beginToEnd < endToEnd then 
                    leftBegin 
                else
                    leftEnd;
            else

            //  (3) Discover the position of the intersection point along line A-B.
            let ABpos = secondEndXRotated + ( secondBeginXRotated - secondEndXRotated ) * secondEndYRotated / ( secondEndYRotated - secondBeginYRotated );

            //  (4) Apply the discovered position to line A-B in the original coordinate system.
            let x = firstBegin.X + ABpos * theCos;
            let y = firstBegin.Y + ABpos * theSin;
            Vector2( ( float32 ) x, ( float32 ) y );
