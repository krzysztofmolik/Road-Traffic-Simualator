using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Components.SimulationMode.Controlers
{
    public class ControlersBase<TElement> : IControlers where TElement : IRoadElement
    {
        public ControlersBase()
        {
            this.Elements = new List<TElement>();
        }

        protected List<TElement> Elements { get; private  set; }

        public virtual void AddControl(IRoadElement element)
        {
            if( typeof(TElement) == element.GetType())
            {
                this.Elements.Add((TElement) element);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual int Order { get { return (int) Infrastructure.Order.Normal; } }
    }
}