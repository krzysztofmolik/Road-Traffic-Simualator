using System;
using System.Collections.Generic;
using Autofac;
using RoadTrafficSimulator.Infrastructure.Controls;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public class DeserializationContext
    {
        private readonly List<IControl> _createdControls = new List<IControl>();
        private readonly CreatedObjects _createdObjects = new CreatedObjects();

        private readonly ILifetimeScope _ioc;
        private readonly IContentManagerAdapter _contentManager;

        public DeserializationContext( ILifetimeScope ioc, IContentManagerAdapter contentManager )
        {
            this._ioc = ioc;
            this._contentManager = contentManager;
        }

        public List<IControl> CreateControls { get { return this._createdControls; } }

        public IControl GetById( Guid controlId )
        {
            return this.CreateControls.FirstOrDefault( c => c.Id == controlId );
        }

        public ILifetimeScope IoC { get { return this._ioc; } }
        public IContentManagerAdapter ContentManager { get { return this._contentManager; } }
        public CreatedObjects CreatedObjects { get { return this._createdObjects; } }

    }
}