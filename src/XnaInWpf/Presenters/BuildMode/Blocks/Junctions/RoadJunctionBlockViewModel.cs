using System;
using System.Windows;
using Common;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.CarsInserter;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Common;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Messages;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.Junctions
{
    public class RoadJunctionBlockViewModel : IBlockViewModel
    {
        private readonly MainBlockViewModel _mainBlockViewModel;
        private readonly IEventAggregator _eventAggreator;
        private readonly NameWithIconViewModel _preview;
        private readonly JunctionTypeViewModel[] _commands;

        public RoadJunctionBlockViewModel( MainBlockViewModel mainBlockViewModel, IEventAggregator eventAggreator )
        {
            this._mainBlockViewModel = mainBlockViewModel;
            this._eventAggreator = eventAggreator;
            this._preview = new NameWithIconViewModel( this.Name, "" );
            this._commands = new[]
                                 {
                                     new JunctionTypeViewModel {CommandType = CommandType.InsertRoadJunction_OneBlock, Name = "One block"},
                                     new JunctionTypeViewModel {CommandType = CommandType.InsertRoadJunction_FourBlocks, Name = "Four blocks"},
                                     new JunctionTypeViewModel { CommandType = CommandType.InsertRoadJunction_TwoBlocksVerticaly, Name = "Two blocks vertically" },
                                     new JunctionTypeViewModel { CommandType = CommandType.InsertRoadJunction_TwoBlocksHorizotaly, Name = "Two blocks horizontally" },
                                 };
        }

        public JunctionTypeViewModel[] Commands { get { return this._commands; } }

        public void Execute( RoutedEventArgs obj )
        {
            var elment = obj.Source as FrameworkElement;
            if ( elment == null ) { return; }

            var dataContext = elment.DataContext as JunctionTypeViewModel;
            if ( dataContext == null ) { return; }

            this._eventAggreator.Publish( new ExecuteCommand( dataContext.CommandType ) );
        }

        public object Preview
        {
            get { return this._preview; }
        }

        public string Name
        {
            get { return "Junction block"; }
        }

        public void GoBack()
        {
            this._eventAggreator.Publish( new ChangeBlock( this._mainBlockViewModel ) );
        }

        public void Execute()
        {
            this._eventAggreator.Publish( new ChangeBlock( this ) );
        }
    }

    public class JunctionTypeViewModel
    {
        public string Name { get; set; }
        public CommandType CommandType { get; set; }
    }
}