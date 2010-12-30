using System;

namespace Common
{
    public class UnitOfWork : IDisposable
    {
        protected Action _beginAction;
        protected Action _endAction;

        public UnitOfWork( Action beginAction, Action endAction )
        {
            this._beginAction = beginAction;
            this._endAction = endAction;

            if( this._beginAction != null )
            {
                this._beginAction();
            }
        }

        public void Dispose()
        {
            if( this._endAction != null )
            {
                this._endAction();
            }
        }
    }
}