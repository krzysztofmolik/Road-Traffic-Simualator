namespace Common.Xna

open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework

    [<System.Runtime.CompilerServices.Extension>]
    module Vector2Exntension =

        [<System.Runtime.CompilerServices.Extension>]   
        let Equal( ( left : Vector2 ), (right : Vector2), epsilon : single ) =
            let xDelta = System.Math.Abs( left.X - right.X )
            let yDelta = System.Math.Abs( left.Y - right.Y )
            let isSmal( value ) = value < epsilon

            isSmal( xDelta) && isSmal( yDelta )

        [<System.Runtime.CompilerServices.Extension>]   
        let angel( left:Vector2, right:Vector2 ) = 
            let leftAngel = System.Math.Atan2( (float)left.Y, (float)left.X )
            let rightAngel = System.Math.Atan2( (float)right.Y, (float)right.X )

            leftAngel - rightAngel
