using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace XnaInWpf.Presenters
{
    public class MyModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<T>( Expression<Func<T>> acessor )
        {
            var memberExpression = acessor.Body as MemberExpression;
            Debug.Assert( memberExpression != null, "memberExpression != null" );

            var propertyName = memberExpression.Member.Name;
            this.RaisePropertyChanged( propertyName );
        }

        private void RaisePropertyChanged( string propertyName )
        {
            var @event = this.PropertyChanged;
            if( @event != null )
            {
                @event( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

    }
}