﻿using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    public abstract class SingleControl<TVertex> : ControlBaseBase<TVertex>, ISingleControl
    {
    }
}