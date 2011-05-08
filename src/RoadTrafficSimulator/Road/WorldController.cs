using System;
using Common;
using RoadTrafficSimulator.Utils;

namespace RoadTrafficSimulator.Road
{
    public class WorldController
    {
        private readonly Camera3D _camera;

        public WorldController( Camera3D camera )
        {
            this._camera = camera.NotNull();
        }

        public BuildModeMainComponent BuildModeMainComponent { get; set; }

        public void SetZoom( float zoom )
        {
            this._camera.Zoom = zoom;
        }

        public float GetZoom()
        {
            return this._camera.Zoom;
        }
    }
}