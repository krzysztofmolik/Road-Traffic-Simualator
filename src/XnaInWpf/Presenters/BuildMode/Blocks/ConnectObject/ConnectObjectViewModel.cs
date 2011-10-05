using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.CarsInserter;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Common;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Junctions;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Messages;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ConnectObjectViewModel : IBlockViewModel
    {
        private readonly MainBlockViewModel _mainBlockViewModel;
        private readonly IEventAggregator _eventAggreator;
        private readonly NameWithIconViewModel _preview;

        public ConnectObjectViewModel( MainBlockViewModel mainBlockViewModel, IEventAggregator eventAggreator )
        {
            this._mainBlockViewModel = mainBlockViewModel;
            this._eventAggreator = eventAggreator;
            this._preview = new NameWithIconViewModel( this.Name, "" );
        }

        public object Preview
        {
            get { return this._preview; }
        }

        public string Name
        {
            get { return "Connect object"; }
        }

        public void GoBack()
        {
            this._eventAggreator.Publish( new ChangeBlock( this._mainBlockViewModel ) );
        }

        public void Execute()
        {
            this._eventAggreator.Publish( new ExecuteCommand( CommandType.ConnectObject ) );
        }

    }
}