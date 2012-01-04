using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Infrastructure.Controls
{
    public interface IRouteElement  : IControl
    {
        IEnumerable<IRouteElement> GetConnectedControls();
    }

    public interface IComponent
    {
        IRouteElement Parent { get; }
    }

    public interface ILogicControl
    {
        Vector2 Location { get; }
        IObservable<bool> SelectionChanged { get; }
        bool IsSelected { get; set; }
        void Redraw();
        void Invalidate();
    }

    [Flags]
    public enum PriorityType
    {
        None = 0,
        Light = 1,
        FromLeft = 2,
        FromFront = 4,
        FromRight = 8,
    }

    public interface IControl : ILogicControl
    {
        IObservable<TranslationChangedEventArgs> Translated { get; }
        IVertexContainer VertexContainer { get; }
        IMouseHandler MouseHandler { get; }
        IObservable<Unit> Redrawed { get; }
        void Translate( Matrix matrixTranslation );
        void TranslateWithoutNotification( Matrix translationMatrix );
        Vector2 ToControlPosition( Vector2 screenPosition );
        bool IsHitted( Vector2 location );
        ILogicControl GetHittedControl( Vector2 point );
        Guid Id { get; set; }
    }

    public interface ISingleControl : IControl
    {
    }
}