using System;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Road.Controls
{
    public interface IEdgeLine : ILogicControl
    {
        MovablePoint StartPoint { get; }
        MovablePoint EndPoint { get; }
        IObservable<TranslationChangedEventArgs> Translated { get; }
    }
}