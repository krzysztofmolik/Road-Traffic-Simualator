using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.RoadJoiners;

namespace XnaRoadTrafficConstructor.Road.RoadJoiners
{
    public interface IRoadJunctionBlock : ICompostControlBase
    {
        Vector2 LeftTopLocation { get; }
        Vector2 RightTopLocation { get; }
        Vector2 LeftBottomLocation { get; }
        Vector2 RightBottomLocation { get; }
        RoadJunctionEdge[] RoadJunctionEdges { get; }
        MovablePoint[] Points { get; }
        RoadJunctionBlockConnection ConnectableObject { get; }
        MovablePoint LeftBottom { get; set; }
        MovablePoint RightBottom { get; set; }
        MovablePoint RightTop { get; set; }
        MovablePoint LeftTop { get; set; }
        MovablePoint CornerHitTest( Vector2 point );
    }

}