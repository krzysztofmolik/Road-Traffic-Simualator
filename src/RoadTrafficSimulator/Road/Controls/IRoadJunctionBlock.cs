using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;

namespace RoadTrafficSimulator.Road.Controls
{
    public interface IRoadJunctionBlock : ICompositeControl
    {
        Vector2 LeftTopLocation { get; }

        Vector2 RightTopLocation { get; }

        Vector2 LeftBottomLocation { get; }

        Vector2 RightBottomLocation { get; }

        RoadJunctionEdge[] RoadJunctionEdges { get; }

        MovablePoint[] Points { get; }
        
        MovablePoint LeftBottom { get; set; }
        
        MovablePoint RightBottom { get; set; }

        MovablePoint RightTop { get; set; }

        MovablePoint LeftTop { get; set; }

        MovablePoint CornerHitTest( Vector2 point );
    }
}