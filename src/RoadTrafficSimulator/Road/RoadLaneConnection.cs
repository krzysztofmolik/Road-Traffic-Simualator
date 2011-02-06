using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Road
{
    public class RoadConnectionEdge : Edge
    {
        private readonly RoadConnectionConnector _connector;
        private readonly IControl _parent;

        public RoadConnectionEdge( Factories.Factories factories, Vector2 location, IControl parent )
            : base( factories )
        {
            this._parent = parent;
            this._connector = new RoadConnectionConnector( this );
            this.StartPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.EndPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
        }

        public RoadConnectionConnector Connector
        {
            get { return this._connector; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }

        public override void Invalidate()
        {
            var connectedRoadLaneEdge = this.Connector.ConnectedObject.OfType<EndRoadLaneEdge>().ToArray();
            Debug.Assert( connectedRoadLaneEdge.Length <= 2, "connectedRoadLaneEdge.Length <= 2" );
            if ( connectedRoadLaneEdge.Length == 2 )
            {
                this.CalculateNewPointLocation(
                            connectedRoadLaneEdge[ 0 ],
                            connectedRoadLaneEdge[ 1 ] );
            }
            else if ( connectedRoadLaneEdge.Length == 1 )
            {
                this.CalcualteNewPointLocation( connectedRoadLaneEdge[ 0 ] );
            }
        }

        private void CalcualteNewPointLocation( EndRoadLaneEdge endRoadLaneEdge )
        {
        }

        private void CalculateNewPointLocation( EndRoadLaneEdge first, EndRoadLaneEdge second )
        {
            var firstOpositeRoadEdge = this.GetOposisteEdge( first );
            var secondOpositeRoadEdge = this.GetOposisteEdge( second );

            var firstQuadrangle = MyMathHelper.CreateQuadrangleFromLocation(
                                                        firstOpositeRoadEdge.Location,
                                                        this.Location,
                                                        Constans.RoadHeight );

            var secondQuadrangle = MyMathHelper.CreateQuadrangleFromLocation(
                                                    this.Location,
                                                    secondOpositeRoadEdge.Location,
                                                    Constans.RoadHeight );

            var newBeginLocation = MyMathHelper.LineIntersectionMethod(
                                                    firstQuadrangle.LeftTop,
                                                    firstQuadrangle.RightTop,
                                                    secondQuadrangle.LeftTop,
                                                    secondQuadrangle.RightTop );

            var newEndLocation = MyMathHelper.LineIntersectionMethod(
                                                    firstQuadrangle.LeftBottom,
                                                    firstQuadrangle.RightBottom,
                                                    secondQuadrangle.LeftBottom,
                                                    secondQuadrangle.RightBottom );

            this.StartPoint.SetLocation( newEndLocation );
            this.EndPoint.SetLocation( newBeginLocation );
        }

        private EndRoadLaneEdge GetOposisteEdge( EndRoadLaneEdge edge )
        {
            if ( edge == edge.RoadLaneBlockParent.LeftEdge )
            {
                return edge.RoadLaneBlockParent.RightEdge;
            }

            // else
            return edge.RoadLaneBlockParent.LeftEdge;
        }
    }
}