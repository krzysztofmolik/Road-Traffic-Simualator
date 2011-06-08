using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class NormalPointEdgeAdapter : IEdgeLine
    {
        private readonly IEdgeLine _edge;

        public NormalPointEdgeAdapter( IEdgeLine edge, IControl parent )
        {
            this._edge = edge;
            this.Parent = parent;
        }

        public Vector2 Location
        {
            get { return this._edge.Location; }
        }

        public IControl Parent { get; set; }

        public IObservable<bool> SelectionChanged
        {
            get { return this._edge.SelectionChanged; }
        }

        public bool IsSelected
        {
            get { return this._edge.IsSelected; }
            set { this._edge.IsSelected = value; }
        }

        public void Redraw()
        {
            this._edge.Redraw();
        }

        public void Invalidate()
        {
            this._edge.Invalidate();
        }

        public MovablePoint StartPoint
        {
            get { return this._edge.StartPoint; }
        }

        public MovablePoint EndPoint
        {
            get { return this._edge.EndPoint; }
        }

        public IObservable<TranslationChangedEventArgs> Translated
        {
            get { return this._edge.Translated; }
        }

        public void RecalculatePostitionAroundStartPoint()
        {
            this._edge.RecalculatePostitionAroundStartPoint();
        }

        public void RecalculatePostitionAroundEndPoint()
        {
            this._edge.RecalculatePostitionAroundEndPoint();
        }

        public void RecalculatePosition()
        {
            this._edge.RecalculatePosition();
        }
    }
}