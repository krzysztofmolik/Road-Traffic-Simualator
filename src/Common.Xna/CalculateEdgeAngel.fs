namespace Common.Xna

open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework

    type CalculateEdgeAngel( roadHeight ) = 
        let _roadHeight = roadHeight
        let CheckBaseLocation baseLocation = 
            match baseLocation with
                | None -> raise ( System.ArgumentException() ) 
                | _ -> ()

        let CheckPreviousAndNext ( previousLocation, nextLocation ) =
            if Option.isNone( previousLocation ) && Option.isNone( nextLocation ) then 
                raise ( System.ArgumentException() ) 
            else 
                ()

        let OneConnection baseLocation otherLocation =
            Math.parpendicularLine baseLocation otherLocation _roadHeight

        let preparePrevAndNext ( prev, bas, next )  =
            let linePrev = 
                match prev with 
                    | None -> None
                    | Some(x) -> 
                        let line = Math.parpendicularLine x bas _roadHeight 
                                   |> revertTuple
                                   |> Line.ofTuple
                        Some( line )

            let lineNext = 
                match next with
                    | None -> None
                    | Some(x) -> 
                        let line =  Math.parpendicularLineTuple( x,  bas, _roadHeight ) 
                                    |> Line.ofTuple

                        Some( line )

            ( linePrev, bas, lineNext )

        let calculateBaseLocationBasedOnSingleNode( prev, bas, next ) =
            if( Option.isSome( prev ) ) then 
                let prevValue = Option.get( prev )
                parpendicularLineTuple( bas, prevValue.Center, _roadHeight ) |> Line.ofTuple
            else
                let nextValue = Option.get ( next )
                parpendicularLineTuple( bas, nextValue.Center, _roadHeight ) |> revertTuple |> Line.ofTuple 


        let calculateBaseLocationBasedOnAllNode( prev, bas, next ) = 
            let prevValue = Option.get( prev )
            let nextValue = Option.get( next )

            let prevEnd = prevValue |> Line.translate ( bas - prevValue.Center )
            let nextBegin = nextValue |> Line.translate ( nextValue.Center - bas )

            let baseBegin = Math.lineIntersectionMethod( prevValue.Start, prevEnd.Start, nextBegin.Start, nextValue.Start )
            let baseEnd = Math.lineIntersectionMethod( prevValue.End, prevEnd.End, nextBegin.End, nextValue.End )

            Line.ofTuple( baseBegin, baseEnd )


        let calculateBaseLine ( prev, bas, next ) =
            if( Option.isNone( prev ) || Option.isNone( next ) ) then
                calculateBaseLocationBasedOnSingleNode( prev, bas, next )
            else
                calculateBaseLocationBasedOnAllNode( prev, bas, next )

        member this.Calculate( ( prevLocation : Vector2 option), (baseLocation : Vector2 ), ( nextLocation : Vector2 option ) ) =
            CheckPreviousAndNext( prevLocation,  nextLocation )
            ( prevLocation, baseLocation, nextLocation )
                |> preparePrevAndNext
                |> calculateBaseLine