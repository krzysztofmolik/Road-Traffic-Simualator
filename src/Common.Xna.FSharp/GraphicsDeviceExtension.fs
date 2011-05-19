namespace Common.Xna.FSharp

open Microsoft.Xna.Framework.Graphics;

    [<System.Runtime.CompilerServices.Extension>]
    module GraphicsDeviceExtension = 

        [<System.Runtime.CompilerServices.Extension>]   
        let DrawTriangleList( graphicsDevice : GraphicsDevice, vertex ) =
            let length =  ( Array.length vertex ) / 3
            graphicsDevice.DrawUserPrimitives( PrimitiveType.TriangleList, vertex, 0, length )
