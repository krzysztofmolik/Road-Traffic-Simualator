using System;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using RoadTrafficSimulator.Factories;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Connectors.Commands;
using RoadTrafficSimulatorTests.IntegrationTests.Infacstructure;

namespace RoadTrafficSimulatorTests.ConnectionTests
{
    [TestFixture]
    public class ConnectRoadConnectionWithRoadConnectionTests
    {
        private ConnectRoadConnectionWithRoadConnection _connectionCommand;
        private RoadConnection _firstRoadConnection;
        private Factories _factories;
        private RoadConnection _secondRoadConnection;

        [SetUp]
        public void SetUp()
        {
            this._factories = IOC.GetService<Factories>();
            this._connectionCommand = new ConnectRoadConnectionWithRoadConnection();
            this._firstRoadConnection = new RoadConnection(_factories, new Vector2(0, 0), null);
            this._secondRoadConnection = new RoadConnection(_factories, new Vector2(0, 2), null);
        }

        [Test]
        public void Test()
        {
            this._connectionCommand.Connect(this._firstRoadConnection, this._secondRoadConnection);
            Console.WriteLine("Hello" );
        }
    }
}