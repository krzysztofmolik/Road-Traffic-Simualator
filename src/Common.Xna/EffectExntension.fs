namespace Common.Xna

open Microsoft.Xna.Framework.Graphics;

    [<System.Runtime.CompilerServices.Extension>]
    module EffectExntension =

        [<System.Runtime.CompilerServices.Extension>]   
        let Begin( effect : Effect ) =
            let applay ( effectPass: EffectPass ) = effectPass.Apply()
            Seq.iter applay effect.CurrentTechnique.Passes