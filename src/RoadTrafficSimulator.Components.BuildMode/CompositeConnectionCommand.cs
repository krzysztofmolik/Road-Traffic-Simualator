using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class CompositeConnectionCommand
    {
        private readonly IEnumerable<IConnectionCommand> _connectionCommand;

        public CompositeConnectionCommand( IEnumerable<IConnectionCommand> connectionCommand )
        {
            Contract.Requires( connectionCommand != null );
            this._connectionCommand = connectionCommand;
        }

        public bool Connect( ILogicControl first, ILogicControl second )
        {
            Contract.Requires( first != null );
            Contract.Requires( second != null );
            var connector = this._connectionCommand.FirstOrDefault( s => s.Connect( first, second ) );
            return connector != null;
        }
    }
}