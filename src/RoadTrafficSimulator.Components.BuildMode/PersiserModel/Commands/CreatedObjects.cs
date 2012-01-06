using System;
using System.Collections.Generic;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public class CreatedObjects
    {
        private readonly IDictionary<Guid, object> _createdObjects = new Dictionary<Guid, object>();

        public void Add( Guid id, object value )
        {
            this._createdObjects.Add( id, value );
        }

        public object Get( Guid id )
        {
            return this._createdObjects[ id ];
        }

    }
}