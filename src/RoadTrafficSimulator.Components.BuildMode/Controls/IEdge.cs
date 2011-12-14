using System;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public interface IEdge : IComponent
    {
        MovablePoint StartPoint { get; }
        MovablePoint EndPoint { get; }
        IObservable<TranslationChangedEventArgs> Translated { get; }
    }
}