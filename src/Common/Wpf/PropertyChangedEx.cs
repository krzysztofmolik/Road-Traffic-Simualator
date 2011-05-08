using System.ComponentModel;
using System.Linq.Expressions;
using System;

namespace Common.Wpf
{
    public static class PropertyChangedEx
    {
        public static void Raise<TRetValue, TSender>( this PropertyChangedEventHandler @event, TSender sender, Expression<Func<TSender, TRetValue>> action )
        {
            var memberExpression = action.Body as MemberExpression;
            if ( memberExpression == null ) { throw new ArgumentException(); }

            var handler = @event;
            if ( handler != null )
            {
                @event( sender, new PropertyChangedEventArgs( memberExpression.Member.Name ) );
            }
        }

    }
}