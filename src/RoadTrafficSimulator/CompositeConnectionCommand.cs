using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator
{
    public class CompositeConnectionCommand 
    {
        private readonly IEnumerable<IConnectionCommand> _connectionCommand;

        public CompositeConnectionCommand( IEnumerable<IConnectionCommand> connectionCommand )
        {
            this._connectionCommand = connectionCommand;
        }

        public bool Connect( IControl first, IControl second )
        {
            var connector = this._connectionCommand.FirstOrDefault( s => s.Connect( first, second ) );
            return connector != null;
        }
    }
}