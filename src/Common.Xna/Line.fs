namespace Common.Xna

open Microsoft.Xna.Framework

    [<AutoOpen>]
    module LineModule =

        type Line = 
            {
                Start : Vector2; 
                End : Vector2
                Center : Vector2
            }
        with
            static member ofTuple ( tuple : Vector2 * Vector2 ) = 
                let inCneter =
                    let second = snd( tuple )
                    let halfLenght = 
                        fst( tuple ) - snd( tuple )
                        |> (*) 0.5f
                        |> (+) second
                    halfLenght

                { 
                    Start = fst( tuple )
                    End = snd( tuple )
                    Center = inCneter 
                }

            static member translate transtationVector line = 
                {
                    Start = line.Start + transtationVector
                    End = line.End + transtationVector
                    Center = line.Center + transtationVector
                }

            member x.toTuple =
                ( x.Start, x.End )

        let revertLine line = 
            { 
                Start = line.End
                End = line.Start 
                Center = line.Center
            }

        let revertTuple ( first, second ) =( second, first )

