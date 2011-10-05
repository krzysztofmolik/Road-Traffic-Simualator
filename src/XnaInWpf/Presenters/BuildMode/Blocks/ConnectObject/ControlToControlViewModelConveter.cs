using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ControlToControlViewModelConveter
    {
        public ControlViewModel Convert( IControl control )
        {
            return new ControlViewModel( control );
        }
    }
}