using System.Threading;

namespace Common
{
    // NOTE Event aggregator taken from Caliburn.Micro
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IHandle{}

    public interface IHandle<in TMessage> : IHandle
    {
        void Handle(TMessage message);
    }

    public interface IEventAggregator
    {
        void Subscribe( object instance );

        void Unsubscribe( object instance );

        void Publish<T>( T message );
    }

    public class EventAggregator : IEventAggregator
    {
        readonly List<WeakReference> subscribers = new List<WeakReference>();

        public void Subscribe( object instance )
        {
            lock ( subscribers )
            {
                if ( subscribers.Any( reference => reference.Target == instance ) )
                    return;

                subscribers.Add( new WeakReference( instance ) );
            }
        }

        public void Unsubscribe( object instance )
        {
            lock ( subscribers )
            {
                var found = subscribers
                    .FirstOrDefault( reference => reference.Target == instance );

                if ( found != null )
                    subscribers.Remove( found );
            }
        }

        public void Publish<TMessage>( TMessage message )
        {
            WeakReference[] toNotify;
            lock ( subscribers )
                toNotify = subscribers.ToArray();

            ThreadPool.QueueUserWorkItem(o =>
            {
                var dead = new List<WeakReference>();

                foreach ( var reference in toNotify )
                {
                    var target = reference.Target as IHandle<TMessage>;

                    if ( target != null )
                        target.Handle( message );
                    else if ( !reference.IsAlive )
                        dead.Add( reference );
                }
                if ( dead.Count > 0 )
                {
                    lock ( subscribers )
                        dead.ForEach( x => subscribers.Remove( x ) );
                }
            } );
        }
    }
}