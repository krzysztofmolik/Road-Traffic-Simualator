using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoadTrafficSimulator.Infrastructure
{
    public class MetaBase
    {
        private readonly Dictionary<string, PropertyInfo> _propertyInfos;

        public MetaBase( IDictionary<string, object> properties )
        {
            // NOTE This is stupid, becouse i use properties from derviced class in base class, but ....
            this._propertyInfos = this.GetType().GetProperties().ToDictionary( s => s.Name, s => s );
            foreach ( var setProperty in properties )
            {
                this.TrySet( setProperty );
            }
        }

        private void TrySet( KeyValuePair<string, object> setProperty )
        {
            var propertyInfo = this.GetPropertyInfo( setProperty.Key );
            if ( propertyInfo == null ) { return; }
            propertyInfo.SetValue( this, setProperty.Value, null );
        }

        private PropertyInfo GetPropertyInfo( string key )
        {
            var propertyInfo = default( PropertyInfo );
            this._propertyInfos.TryGetValue( key, out propertyInfo );
            return propertyInfo;
        }
    }
}