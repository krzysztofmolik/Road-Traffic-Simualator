using System;

namespace RoadTrafficSimulator.Infrastructure.Messages
{
    public class InsertControl
    {
        public InsertControl( Type controlType )
        {
            this.ControleType = controlType;
        }

        public Type ControleType { get; private set; }
    }
}