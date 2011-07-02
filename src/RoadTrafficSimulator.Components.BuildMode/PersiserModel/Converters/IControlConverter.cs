using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public interface IControlConverter
    {
        Type Type { get; }
        IEnumerable<IAction> ConvertToAction( IControl control );
    }
}