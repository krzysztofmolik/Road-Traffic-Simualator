using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class PriorityMouseInfomrmation
    {
        private readonly IList<FilterMouseInformation> _observerStack = new List<FilterMouseInformation>();

        public PriorityMouseInfomrmation( IMouseInformation mouseInfomration )
        {
            this.MouseInformation = mouseInfomration;

            this.MouseInformation.LeftButtonClicked.
                Subscribe( mouseState => this.ExecuteOnTopOfStack( s => s.LeftButtonClickedSubject.OnNext( mouseState ) ) );

            this.MouseInformation.LeftButtonPressed.
                Subscribe( mouseState => this.ExecuteOnTopOfStack( s => s.LeftButtonPressedSubject.OnNext( mouseState ) ) );

            this.MouseInformation.LeftButtonRelease.
                Subscribe( mouseState => this.ExecuteOnTopOfStack( s => s.LeftButtonReleaseSubject.OnNext( mouseState ) ) );

            this.MouseInformation.MousePositionChanged.
                Subscribe( mouseState => this.ExecuteOnTopOfStack( s => s.MousePositionChangedSubject.OnNext( mouseState ) ) );

            this.MouseInformation.ScrollWheelChanged
                .Subscribe( mousState => this.ExecuteOnTopOfStack( s => s.ScrollWheelValueDeltaSubject.OnNext( mousState ) ) );

            this.MouseInformation.DoubleClick
                .Subscribe( mousState => this.ExecuteOnTopOfStack( s => s.DoubleClickedSubject.OnNext( mousState ) ) );
        }

        public IMouseInformation MouseInformation { get; private set; }

        private void ExecuteOnTopOfStack( Action<FilterMouseInformation> action )
        {
            var lastAdded = this._observerStack.LastOrDefault();
            if ( lastAdded == null )
            {
                return;
            }

            action( lastAdded );
        }

        public void Push( FilterMouseInformation filterMouseInformation )
        {
            this._observerStack.Add( filterMouseInformation );
        }

        public void Pull( FilterMouseInformation filterMouseInformation )
        {
            this._observerStack.Remove( filterMouseInformation );
        }
    }
}