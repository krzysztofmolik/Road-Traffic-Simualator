using System;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.Factories
{
    public class ControlFactories : IControlFactories
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<Vector2, JunctionEdge> _edgeFactory;
        private readonly Func<Vector2, RoadJunctionBlock> _junctionFactroy;

        public ControlFactories( IEventAggregator eventAggregator, Func<Vector2, JunctionEdge> edgeFactory, Func<Vector2, RoadJunctionBlock> junctionFactroy )
        {
            this._eventAggregator = eventAggregator;
            this._edgeFactory = edgeFactory;
            this._junctionFactroy = junctionFactroy;
        }

        public RoadJunctionBlock CreateRoadJunctioBlockWithEdges( Vector2 location )
        {
            var leftEdge = _edgeFactory( location );
            var rightEdge = _edgeFactory( location );
            var topEdge = _edgeFactory( location );
            var bottomEdge = _edgeFactory( location );

            var junction = this._junctionFactroy( location );

            junction.Connector.ConnectStartOn( leftEdge, EdgeType.Left );
            leftEdge.Connector.ConnectWithJunction( junction, EdgeType.Left );

            junction.Connector.ConnectStartOn( rightEdge, EdgeType.Right );
            rightEdge.Connector.ConnectWithJunction( junction, EdgeType.Right );

            junction.Connector.ConnectStartOn( topEdge, EdgeType.Top );
            topEdge.Connector.ConnectWithJunction( junction, EdgeType.Top );

            junction.Connector.ConnectStartOn( bottomEdge, EdgeType.Bottom );
            bottomEdge.Connector.ConnectWithJunction( junction, EdgeType.Bottom );


            this._eventAggregator.Publish( new NewControlCreated( junction ) );
            this._eventAggregator.Publish( new NewControlCreated( leftEdge ) );
            this._eventAggregator.Publish( new NewControlCreated( rightEdge ) );
            this._eventAggregator.Publish( new NewControlCreated( bottomEdge ) );
            this._eventAggregator.Publish( new NewControlCreated( topEdge ) );

            junction.Invalidate();
            leftEdge.Invalidate();
            rightEdge.Invalidate();
            bottomEdge.Invalidate();
            topEdge.Invalidate();

            return junction;
        }
    }
}