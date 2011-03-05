using System;
using System.Threading;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using RoadTrafficSimulator.Factories;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Connectors.Commands;
using RoadTrafficSimulator.Road.Controls;
using RoadTrafficSimulatorTests.IntegrationTests.Infacstructure;
using XnaRoadTrafficConstructor.Road;

namespace RoadTrafficSimulatorTests.ConnectionTests
{
    [TestFixture]
    public class RoadJunctionEdgeWithEndRoadLaneEdgeConnectionTest
    {
        private Factories _factories;
        private RoadJunctionBlock _roadJunctionBlock;
        private RoadConnection _connection;
        private RoadLaneBlock _roadLane;
        private ConnectEndRoadLaneEdgeWithRoadLaneConnection _edgeConnectionConnecto;
        private ConnectRoadJunctionEdgeWitEndRoadLaneEdge _roadJunctionConnector;

        [SetUp]
        public void Setup()
        {
            this._factories = IOC.GetService<Factories>();
            this._roadJunctionBlock = new RoadJunctionBlock( this._factories, new Vector2( -0.05f, 0.05f ), null );
            this._connection = new RoadConnection( this._factories, new Vector2( 1.0f, 0.05f ), null );
            this._roadLane = new RoadLaneBlock( this._factories, null );

            this._roadJunctionConnector = new ConnectRoadJunctionEdgeWitEndRoadLaneEdge();
            this._edgeConnectionConnecto = new ConnectEndRoadLaneEdgeWithRoadLaneConnection();

        }

        [Test]
        public void Test()
        {
            var firstConnectionSuccess = this._roadJunctionConnector.Connect(
                                                this._roadJunctionBlock.RoadJunctionEdges[ EdgeType.Right ],
                                                this._roadLane.LeftEdge );
            var secondConnectionSuccess = this._edgeConnectionConnecto.Connect( this._roadLane.RightEdge, this._connection );

            Assert.That( firstConnectionSuccess, Is.True );
            Assert.That( secondConnectionSuccess, Is.True );

            Assert.That( this._connection.StartLocation, Is.EqualTo( new Vector2( 1.0f, 0.0f ) ) );
            Assert.That( this._connection.EndLocation, Is.EqualTo( new Vector2( 1.0f, 0.1f ) ) );
        }

        [Test]
        public void Test2()
        {

            this._roadJunctionConnector.Connect(
                this._roadJunctionBlock.RoadJunctionEdges[ EdgeType.Right ],
                this._roadLane.LeftEdge );
            this._edgeConnectionConnecto.Connect( this._roadLane.RightEdge, this._connection );

            this._connection.Translate( Matrix.CreateTranslation( 1.0f, 0.0f, 0.0f ) );

            Assert.That( this._connection.StartPoint.Location, Is.EqualTo( new Vector2( 2.0f, 0.0f ) ) );
            Assert.That( this._connection.EndPoint.Location, Is.EqualTo( new Vector2( 2.0f, 0.1f ) ) );

        }
    }
}