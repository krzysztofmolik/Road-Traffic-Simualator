using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class NormalEdge : Edge
    {
        private readonly IControl _parent;

        public NormalEdge( Factories.Factories factories, IControl parent, Vector2 location )
            : base( factories, Styles.NormalStyle, parent )
        {
            _parent = parent;
            this.StartPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.EndPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
        }

        public IControl Parent
        {
            get { return this._parent; }
        }
    }
}