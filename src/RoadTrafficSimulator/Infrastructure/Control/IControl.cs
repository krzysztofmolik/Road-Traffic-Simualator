using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    public interface IControl
    {
        bool IsSelected { get; set; }

        IObservable<Unit> Changed { get; }

        IVertexContainer VertexContainer { get; }

        IMouseSupport MouseSupport { get; }

        Vector2 Location { get; }

        IEnumerable<IControl> Parents { get; }

        IObservable<bool> IsSelectedChanged { get; }

        ISelectionSupport SelectionSupport { get; }

        void Translate( Matrix matrixTranslation );

        Vector2 ToControlPosition( Vector2 screenPosition );

        IControl GetRoot();

        bool HitTest( Vector2 point );

        void Invalidate();
    }

    public interface ISingleControl : IControl
    {
    }
}