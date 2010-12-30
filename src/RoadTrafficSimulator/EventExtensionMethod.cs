using System;

namespace XnaVs10
{
    public static class EventExtensionMethod
    {
        public static void Raise(this EventHandler @event, object sender )
        {
            Raise(@event, sender, EventArgs.Empty);
        }

        public static void Raise(this EventHandler @event, object sender, EventArgs e )
        {
            var handler = @event;
            if( handler!= null )
            {
                @event(sender, e);
            }
        }

        public static void Raise<T>(this EventHandler<T> @event, object sender, T args) 
            where T : EventArgs
        {
            var handler = @event;
            if( handler != null)
            {
                @event.Invoke(sender, args);
            }
        }
    }
}