using System;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class InsertRoadJunctionFourBlocksCommand : ICommand
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly Factories.Factories _factories;

        public InsertRoadJunctionFourBlocksCommand( IMouseInformation mouseInformation, Factories.Factories factories, IEventAggregator eventAggregator )
        {
            this._factories = factories;
            this._mouseInformation = mouseInformation.NotNull();

            this._mouseInformation.LeftButtonPressed.Subscribe( this.AddJunction );
        }

        private void AddJunction( XnaMouseState mouseState )
        {
            const float halfRoadSize = Constans.RoadHeight / 2;
            var leftTop = this._factories.ControlFactory.CreateRoadJunctioBlockWithEdges( mouseState.Location - new Vector2( halfRoadSize, -halfRoadSize ) );
            var rightTop = this._factories.ControlFactory.CreateRoadJunctioBlockWithEdges( mouseState.Location - new Vector2( -halfRoadSize, -halfRoadSize ) );
            var rightBottom = this._factories.ControlFactory.CreateRoadJunctioBlockWithEdges( mouseState.Location - new Vector2(-halfRoadSize, halfRoadSize ) );
            var leftBottom = this._factories.ControlFactory.CreateRoadJunctioBlockWithEdges( mouseState.Location - new Vector2( halfRoadSize, halfRoadSize ) );


            leftTop.Connector.BottomEdge.Connector.ConnectBeginFrom( leftBottom.Connector.TopEdge );
            leftBottom.Connector.TopEdge.Connector.ConnectEndsOn( leftTop.Connector.BottomEdge );

            leftTop.Connector.RightEdge.Connector.ConnectBeginFrom( rightTop.Connector.LeftEdge );
            rightTop.Connector.LeftEdge.Connector.ConnectEndsOn( leftTop.Connector.RightEdge );

            rightTop.Connector.BottomEdge.Connector.ConnectBeginFrom( rightBottom.Connector.TopEdge );
            rightBottom.Connector.TopEdge.Connector.ConnectEndsOn( rightTop.Connector.BottomEdge );

            rightBottom.Connector.LeftEdge.Connector.ConnectBeginFrom( leftBottom.Connector.RightEdge );
            leftBottom.Connector.RightEdge.Connector.ConnectEndsOn( rightBottom.Connector.LeftEdge );
        }

        public CommandType CommandType
        {
            get { return CommandType.InsertRoadJunction_FourBlocks; }
        }

        public void Start()
        {
            this._mouseInformation.StartRecord();
        }

        public void Stop()
        {
            this._mouseInformation.StopRecord();
        }
    }
}