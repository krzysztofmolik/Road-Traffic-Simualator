using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Common;
using Microsoft.Xna.Framework.Input;
using NLog;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Messages;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class BuilderCommandManager : IHandle<ExecuteCommand>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly KeyboardInputNotify _keyboardInformation;
        private readonly ICommand[] _creators;
        private readonly IEventAggregator _eventAggregator;
        // TODO Check lazy
        public BuilderCommandManager( KeyboardInputNotify keyboardInformation, IEnumerable<ICommand> creators, IEventAggregator eventAggregator )
        {
            Contract.Ensures( this._keyboardInformation != null );
            Contract.Ensures( this._eventAggregator != null );
            this._keyboardInformation = keyboardInformation;
            this._creators = creators.ToArray();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.Subscribe( this );

            this.SubscribeMessages();
        }

        private void SubscribeMessages()
        {
            this._keyboardInformation.KeyRelease.Where( s => s.Key == Keys.Escape )
                .Subscribe( s => this.CancelAllOperation() );
        }

        private void CancelAllOperation()
        {
            this._creators.ForEach( s => s.Stop() );
        }

        public void Handle( ExecuteCommand message )
        {
            this._creators.ForEach( c => c.Stop() );
            var creator = this._creators.FirstOrDefault( s => s.CommandType == message.CommandType );
            if ( creator == null )
            {
                _logger.Warn( "Can't find command. Type: {0}", message.CommandType );
                return;
            }

            creator.Start();
        }
    }
}