using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Road;

namespace XnaVs10.Road
{
    public class ConetableBase<TJoiner> : IConnectable<TJoiner>
    {
        private readonly IList<IConnectable<TJoiner>> _connectedObjects;
        private readonly Subject<IConnectable<TJoiner>> _connected;
        private readonly Subject<IConnectable<TJoiner>> _disconnected;

        public ConetableBase()
        {
            this._connectedObjects = new List<IConnectable<TJoiner>>();
            this._connected = new Subject<IConnectable<TJoiner>>();
            this._disconnected = new Subject<IConnectable<TJoiner>>();
        }

        public void Connect( IConnectable<TJoiner> connectable )
        {
            this._connectedObjects.Add( connectable );
            connectable.NotifyAboutConnection( this );
        }

        public IObservable<IConnectable<TJoiner>> Disconnected { get { return this._disconnected; } }
        public IObservable<IConnectable<TJoiner>> Connected { get { return this._connected; } }

        public void NotifyAboutConnection( IConnectable<TJoiner> connectable )
        {
            this._connectedObjects.Add( connectable );
            this._connected.OnNext( connectable );
        }

        public virtual void NotifyAboutDisconnected( IConnectable<TJoiner> connectable )
        {
            this._connectedObjects.Remove( connectable );
            this._disconnected.OnNext( connectable );
        }

        public void DisconnectAll()
        {
            foreach ( var conectable in this._connectedObjects )
            {
                conectable.NotifyAboutDisconnected( this );
            }
        }

    }
}