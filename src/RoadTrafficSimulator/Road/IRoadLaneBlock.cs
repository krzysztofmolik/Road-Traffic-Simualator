using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.RoadJoiners;
using XnaRoadTrafficConstructor.Road.RoadJoiners;

namespace RoadTrafficSimulator.Road
{
    public interface IRoadLaneBlock : ICompostControlBase
    {
        event EventHandler VectorChanged;
        Vector2 BeginLocation { get; }
        Vector2 EndLocation { get; }

        IList<IControl> RoadBlocks { get; }
        Vector2 LeftTopLocation { get; }
        Vector2 LeftBottomLocation { get; }
        Vector2 RightTopLocation { get; }
        Vector2 RightBottomLocation { get; }
        MovablePoint LeftTopPoint { get; set; }
        MovablePoint RightTopPoint { get; set; }
        MovablePoint RightBottomPoint { get; set; }
        MovablePoint LeftBottomPoint { get; set; }
        EndRoadLaneEdge LeftEdge { get; }
        EndRoadLaneEdge RightEdge { get; }
        SideRoadLaneEdge TopEdge { get; }
        SideRoadLaneEdge BottomEdge { get; }
        IEnumerable<IControl> HitRoadBlock( Vector2 point );
    }
}