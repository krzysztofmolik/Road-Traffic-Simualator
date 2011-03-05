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

            ( xDelta |> isSmal && yDelta |> isSmal )
