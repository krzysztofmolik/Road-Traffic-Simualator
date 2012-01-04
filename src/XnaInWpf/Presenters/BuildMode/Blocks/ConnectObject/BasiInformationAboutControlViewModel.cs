using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class BasiInformationAboutControlViewModel
    {
        private readonly IControl _control;
        private readonly string _controlType;

        public BasiInformationAboutControlViewModel( IControl control )
        {
            this._control = control;
            this._controlType = this._control.GetType().Name;
        }

        public string ControlType
        {
            get { return this._controlType; }
        }

        public Vector2 Location
        {
            get { return this._control.Location; }
        }

        public IControl Control
        {
            get { return this._control; }
        }
    }
}