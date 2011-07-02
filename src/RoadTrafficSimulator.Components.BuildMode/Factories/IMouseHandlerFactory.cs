using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Components.BuildMode.Factories
{
    public interface IMouseHandlerFactory
    {
        IMouseHandler Create(RoadLayer roadLayer);
        IMouseHandler Create(IControl control);
        IMouseHandler Create(ICompositeControl control);
        IMouseHandler CreateEmpty();
    }

    [Serializable]
    public class MouseHandlerFactory : IMouseHandlerFactory
    {
        private readonly Func<RoadLayer, IMouseHandler> _roadLayerMouseHandlerFactory;
        private readonly Func<IControl, IMouseHandler> _controlMouseHandlerFactory;
        private readonly Func<ICompositeControl, IMouseHandler> _compositeControlMouseHandlerFactory;

        public MouseHandlerFactory(
            Func<RoadLayer, IMouseHandler> roadLayerMouseHandlerFactory,
            Func<IControl, IMouseHandler> controlMouseHandlerFactory,
            Func<ICompositeControl, IMouseHandler> compositeControlMouseHandlerFactory )
        {
            this._roadLayerMouseHandlerFactory = roadLayerMouseHandlerFactory;
            this._controlMouseHandlerFactory = controlMouseHandlerFactory;
            this._compositeControlMouseHandlerFactory = compositeControlMouseHandlerFactory;
        }

        public IMouseHandler Create(RoadLayer roadLayer)
        {
            return this._roadLayerMouseHandlerFactory(roadLayer);
        }

        public IMouseHandler Create(IControl control)
        {
            return this._controlMouseHandlerFactory(control);
        }

        public IMouseHandler Create(ICompositeControl control)
        {
            return this._compositeControlMouseHandlerFactory(control);
        }

        public IMouseHandler CreateEmpty()
        {
            return new NotMovableMouseHandler();
        }
    }
}