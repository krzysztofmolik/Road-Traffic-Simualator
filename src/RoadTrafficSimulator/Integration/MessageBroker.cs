using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Road;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaVs10.Utils;

namespace RoadTrafficSimulator.Integration
{
    public class ErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
    }

    public class MessageBroker
    {
        public MessageBroker()
        {
            this.RoadLaneCreated = new Subject<InstanceArgs<IRoadLaneBlock>>();
            this.RoadLaneDestroyed = new Subject<InstanceArgs<IRoadLaneBlock>>();

            this.LineDrawed = new Subject<Line>();

            this.RoadJunctionBlockCreated = new Subject<InstanceArgs<IRoadJunctionBlock>>();

        }


        public ISubject<InstanceArgs<IRoadLaneBlock>> RoadLaneCreated { get ; private set ; }

        public ISubject<InstanceArgs<IRoadLaneBlock>> RoadLaneDestroyed { get; private set; }

        public ISubject<Line> LineDrawed { get; private set; }

        public ISubject<InstanceArgs<IRoadJunctionBlock>> RoadJunctionBlockCreated { get; private set; }
    }

    public static class InstanceArgs
    {
        public static InstanceArgs<T> Create<T>( T instance )
        {
            return new InstanceArgs<T>( instance );
        }

    }
    public class InstanceArgs<TInstance>
    {
        public InstanceArgs( TInstance instance )
        {
            this.Instance = instance;
        }

        public TInstance Instance { get; private set; }
    }
}