﻿using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road
{
    public class RoadConnectionEdge : ILogicControl
    {
        private readonly RoadConnection _roadConnection;
        private readonly bool _shouldInvert;

        public RoadConnectionEdge(RoadConnection roadConnection, bool shouldInvert)
        {
            this._roadConnection = roadConnection;
            this._shouldInvert = shouldInvert;
        }

        public Vector2 Location
        {
            get { return this._roadConnection.Location; }
        }

        public IControl Parent
        {
            get { return this._roadConnection.Parent; }
        }

        public IObservable<bool> IsSelectedChanged
        {
            get { return this._roadConnection.IsSelectedChanged; }
        }

        public MovablePoint StartPoint
        {
            get { return this._shouldInvert ? this._roadConnection.EndPoint : this._roadConnection.StartPoint; }
        }

        public MovablePoint EndPoint
        {
            get { return this._shouldInvert ? this._roadConnection.StartPoint : this._roadConnection.EndPoint; }
        }
    }
}