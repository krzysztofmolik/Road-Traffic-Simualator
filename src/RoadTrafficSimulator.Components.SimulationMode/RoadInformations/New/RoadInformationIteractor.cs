using System.Collection.Generic;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using System.Diagnostic;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.New
{
    // TODO Change class name and file name
    public class RoadInformationEnumerator : IEnumerator<IRoadInformation>, IEnumerable<IRoadInformation>
    {
        private IRouteMark<IRoadInformation> _route;
        private IRouteMark<IRoadInformation> _orginalRoute;
        private float _maxDistance;
        private float _currentDistance = 0.0f;
        private float _version;

        public RoadInformationEnumerator( IRouteMark<IRoadInformation> route, float maxDistance )
        {
            this._orginalRoute = route;
            this._route = route.Clone();
            this._maxDistance = maxDistance;
            this._version = 0;
        }

        public IRoadInformation Current { get; private set; }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if( this._version == 0 )
            {
                return this.MoveFirstTime();
            }

            return this.MoveNormal();
        }

        public void Reset()
        {
            this._route = this._orginalRoute.Clone();
            this._version = 0;
        }

        public IEnumerator<IRoadInformation> GetEnumerator()
        {
            return new RoadInformationEnumerator ( this._orginalRoute, this._maxDistance);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        private bool MoveFirstTime()
        {
            if( this._route.Current == null ) { return false; }
            this.Current = this._route.Current;
            this._currentDistance = 0.0f;
            return true;
        }
        
        private bool MoveNormal()
        {
            if( this._currentDistance > this._maxDistance ) { return false }

            var moveNext = this._route.MoveNext();
            if( !moveNext ) { return false; }
            this.Current = this._route.Current;
            Debug.Assert) this.Current != null );
            this._currentDistance += this.Current.Length( this._route.GetPrevious(), this._route.GetNext() );
        }
    }
}
