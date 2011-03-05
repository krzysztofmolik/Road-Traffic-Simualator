using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    public interface ILogicControl
    {
        Vector2 Location { get; }

        IControl Parent { get; }

        IObservable<bool> IsSelectedChanged { get; }
    }

    public interface IControl : ILogicControl
    {
        bool IsSelected { get; set; }

        IObservable<TranslationChangedEventArgs> Translated { get; }

        IVertexContainer VertexContainer { get; }

        IMouseSupport MouseSupport { get; }

        IObservable<Unit> Redrawed { get; }

        void Translate( Matrix matrixTranslation );

        Vector2 ToControlPosition( Vector2 screenPosition );

        IControl GetRoot();

        bool HitTest( Vector2 point );
    }

    public interface ISingleControl : IControl
    {
    }
}