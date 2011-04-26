using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road
{
    public class InvertPointEdgeAdapter : IEdgeLine
    {
        private readonly Edge _edge;

        public InvertPointEdgeAdapter( Edge edge )
        {
            this._edge = edge;
        }

        public Vector2 Location
        {
            get { return this._edge.Location; }
        }

        public IControl Parent
        {
            get { return this._edge.Parent; }
            set { this._edge.Parent = value; }
        }

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
            get { return this._edge.EndPoint ; }
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