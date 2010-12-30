module FSharp.Math

open Microsoft.Xna.Framework
open System

let ParpendicularVector ( vector:Vector2 ) ( length : float32 ) =
    let normalized = Vector2.Normalize( vector )
    normalized * length

let ProstopadlaLini ( lineBegin:Vector2) (lineEnd:Vector2) ( lenght:float32) =
    let halfLength = lenght * 0.5f
    let inCenter = lineBegin - lineEnd
    let parpendicualrVector = ParpendicularVector inCenter halfLength 
    let opositeDirection = parpendicualrVector * -1.0f

    lineBegin + parpendicualrVector, lineBegin + opositeDirection

let ToCenter (startPoint : Vector2 ) ( endPoint : Vector2 )  = 
    endPoint - startPoint

let VectorAngel ( startPoint : Vector2 ) ( endPoint : Vector2 ) =
    let inCenter = ToCenter startPoint endPoint 
    Math.Atan2((float)inCenter.X, (float)inCenter.Y )
