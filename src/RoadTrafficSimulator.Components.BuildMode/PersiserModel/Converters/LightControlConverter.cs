using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class LightControlConverter : ControlConverterBase
    {
        public override Type Type
        {
            get { return typeof( LightBlock ); }
        }

        public override IEnumerable<IAction> ConvertToAction( IControl control )
        {
            return this.Convert( ( LightBlock ) control );
        }

        private IEnumerable<IAction> Convert( LightBlock control )
        {
            yield return Actions.CreateControl( control.Id, () => new LightBlock( Is.Const( control.Location ), Is.IsTexture( control.TextureName) ) );

            Debug.Assert( control.Connector.Owner != null );
            yield return Actions.Call<LightBlock>( control.Id, () => control.Connector.ConnectWith( Is.Control( control.Connector.Owner ) ) );
            yield return Actions.Property( control, c => c.Times.GreenLightTime );
            yield return Actions.Property( control, c => c.Times.RedLightTime );
            yield return Actions.Property( control, c => c.Times.YellowLightTime );
            yield return Actions.Property( control, c => c.Times.SetupDealy );
        }
    }
}