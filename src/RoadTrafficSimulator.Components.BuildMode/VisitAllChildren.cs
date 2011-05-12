using System.Collections;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class VisitAllChildren : IEnumerator<IControl>, IEnumerable<IControl>
    {
        private readonly Stack<IEnumerator<IControl>> _compositeControlQueue = new Stack<IEnumerator<IControl>>();
        private readonly IControl _root;
        private IControl _next;

        public VisitAllChildren( IControl root )
        {
            this._root = root;
        }

        public IControl Current { get; private set; }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if ( this._next == null )
            {
                this.GetLastLeaft( this._root );
                this._next = this.GetCurrentOnQueue();
            }

            this.Current = this._next;

            var next = this.GetNextFromQueue();
            if ( next == null )
            {
                if ( this._compositeControlQueue.Count == 0 )
                {
                    return false;
                }

                this._compositeControlQueue.Pop();
                this._next = this.GetCurrentOnQueue();
                return this._next != null;
            }

            this.GetLastLeaft( next );
            this._next = this.GetCurrentOnQueue();
            return true;
        }

        public void Reset()
        {
            this.Current = null;
        }

        public IEnumerator<IControl> GetEnumerator()
        {
            return new VisitAllChildren( this._root );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IControl GetCurrentOnQueue()
        {
            if ( this._compositeControlQueue.Count == 0 )
            {
                return null;
            }

            return this._compositeControlQueue.Peek().Current;
        }

        private IControl GetLastLeaft( IControl fromQueue )
        {
            var compositeControl = fromQueue as ICompositeControl;
            if ( compositeControl == null )
            {
                return fromQueue;
            }

            var enumerator = compositeControl.Children.GetEnumerator();
            if ( enumerator.MoveNext() == false )
            {
                return fromQueue;
            }

            this._compositeControlQueue.Push( enumerator );
            return this.GetLastLeaft( enumerator.Current );
        }

        private IControl GetNextFromQueue()
        {
            var enumeratorFromQueue = this.GetCurentEnumerator();
            if ( enumeratorFromQueue == null )
            {
                return null;
            }

            if ( enumeratorFromQueue.MoveNext() )
            {
                return enumeratorFromQueue.Current;
            }

            return null;
        }

        private IEnumerator<IControl> GetCurentEnumerator()
        {
            if ( this._compositeControlQueue.Count == 0 )
            {
                return null;
            }

            return this._compositeControlQueue.Peek();
        }
    }
}