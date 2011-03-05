using System;
using Autofac;
using Autofac.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WinFormsGraphicsDevice
{
    public class XnaWinFormModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load( builder );
        }
    }
}