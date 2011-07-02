using System;
using System.Collections.Generic;
using Autofac;
using RoadTrafficSimulator.Infrastructure.Controls;
using System.Linq;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public class DeserializationContext
    {
        private readonly List<IControl> _createdControls = new List<IControl>();
        private ILifetimeScope _ioc;

        public DeserializationContext( ILifetimeScope ioc )
        {
            this._ioc = ioc;
        }

        public List<IControl> CreateControls { get { return this._createdControls; } }

        public IControl GetById( Guid controlId )
        {
            return this.CreateControls.FirstOrDefault( c => c.Id == controlId);
        }

        public ILifetimeScope IoC
        {
            get { return this._ioc; }
        }
    }
}