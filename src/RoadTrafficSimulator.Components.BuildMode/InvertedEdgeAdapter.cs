using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class InvertedEdgeAdapter : IEdge
    {
        private readonly IEdge _edge;
        private readonly IRouteElement _parent;

        public InvertedEdgeAdapter( IEdge edge, IRouteElement parent )
        {
            this._edge = edge;
            this._parent = parent;
        }

        public IRouteElement Parent { get { return this._parent; } }

        public MovablePoint StartPoint
        {
            get { return this._edge.EndPoint; }
        }

        public MovablePoint EndPoint
        {
            get { return this._edge.StartPoint; }
        }

        public IObservable<TranslationChangedEventArgs> Translated
        {
            get { return this._edge.Translated; }
        }
    }
}