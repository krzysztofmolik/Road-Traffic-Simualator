using RoadTrafficSimulator.Integration;
using Common;

namespace RoadTrafficSimulator
{
    public class RoadBuilderCommandCenter
    {
        private MessageBroker _messageBroker;

        public RoadBuilderCommandCenter( MessageBroker messageBroker )
        {
            this._messageBroker = messageBroker.NotNull();
        }
    }
}