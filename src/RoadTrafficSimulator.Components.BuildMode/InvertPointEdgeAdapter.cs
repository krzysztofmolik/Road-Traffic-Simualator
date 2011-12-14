using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class InvertPointEdgeAdapter : IEdgeLine,  IEdge
    {
        private readonly IEdgeLine _edgeLine;
        private readonly IEdge _edge;
        private readonly IControl _parent;

        public InvertPointEdgeAdapter( IEdgeLine edgeLine, IEdge edge, IControl parent )
        {
            this._edgeLine = edgeLine;
            this._edge = edge;
            this._parent = parent;
        }

        public IControl Parent { get { return this._parent; } }

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

        public void RecalculatePostitionAroundStartPoint()
        {
            this._edgeLine.RecalculatePostitionAroundEndPoint();
        }

        public void RecalculatePostitionAroundEndPoint()
        {
            this._edgeLine.RecalculatePostitionAroundStartPoint();
        }

        public void RecalculatePosition()
        {
            this._edgeLine.RecalculatePosition();
        }
    }
}