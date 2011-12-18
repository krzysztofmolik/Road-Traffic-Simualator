using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NLog;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public class ControlSerializer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<Type, IControlConverter> _converters;
        private readonly Func<DeserializationContext> _deserializationContextFactory;

        public ControlSerializer( IEnumerable<IControlConverter> converters, Func<DeserializationContext> deserializationContextFactory )
        {
            this._converters = converters.ToDictionary( s => s.Type, s => s );
            this._deserializationContextFactory = deserializationContextFactory;
        }

        public void Save( Stream stream, IEnumerable<IControl> controls )
        {
            var formater = new BinaryFormatter();
            var converterControl = this.Converter( controls ).ToArray();
            formater.Serialize( stream, converterControl );
        }

        private IEnumerable<IAction> Converter( IEnumerable<IControl> controls )
        {
            return controls.SelectMany( control => this._converters[ control.GetType() ].ConvertToAction( control ) );
        }

        public IEnumerable<IControl> Load( Stream stream )
        {
            var formater = new BinaryFormatter();
            var actions = formater.Deserialize( stream ) as IAction[];
            if ( actions == null )
            {
                Logger.Error( "Can't deserialzie stream" );
                throw new ArgumentException( "Can't deserialize stream" );
            }

            var context = this._deserializationContextFactory();
            var orderedActions = actions.OrderBy( s => s.Priority );
            foreach ( var action in orderedActions )
            {
                action.Execute( context );
            }

            // TODO Remove it, temporary solution
            context.CreateControls.ForEach( c => c.Invalidate() );
            return context.CreateControls;
        }
    }
}