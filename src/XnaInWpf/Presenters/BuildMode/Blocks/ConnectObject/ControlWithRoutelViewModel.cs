using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common.Wpf;
using System.Linq;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ControlWithRoutelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IControl _control;
        private readonly string _controlType;
        private readonly ObservableCollection<RouteViewModel> _routes;

        public ControlWithRoutelViewModel( IControl control, IEnumerable<RouteViewModel> route )
        {
            this._control = control;
            this._controlType = this._control.GetType().Name;
            this._routes = new ObservableCollection<RouteViewModel>( route );
            this.Mode = new ExploreMode( this );
        }

        public string ControlType
        {
            get { return this._controlType; }
        }

        public Vector2 Location
        {
            get { return this._control.Location; }
        }

        public ObservableCollection<RouteViewModel> Routes
        {
            get { return this._routes; }
        }

        public IControl Control
        {
            get { return this._control; }
        }

        private IMode _mode;
        public IMode Mode
        {
            get { return this._mode; }
            set
            {
                this._mode = value;
                this.PropertyChanged.Raise( this, () => this.Mode );
            }
        }

        #region Inner classes

        private class AddNewItemMode : IMode
        {
            private ControlWithRoutelViewModel _parent;

            public AddNewItemMode( ControlWithRoutelViewModel parent )
            {
                this._parent = parent;
            }

            public RouteViewModelMode Mode { get { return RouteViewModelMode.AddItemsToRoute; } }

            public void ControlClicked( ControlViewModel controlViewModel )
            {
                throw new NotImplementedException();
            }
        }

        private class ExploreMode : IMode
        {
            private readonly ControlWithRoutelViewModel _parent;

            public ExploreMode( ControlWithRoutelViewModel parent )
            {
                this._parent = parent;
            }

            public RouteViewModelMode Mode { get { return RouteViewModelMode.AddItemsToRoute; } }

            public void ControlClicked( ControlViewModel controlViewModel )
            {
                var foundControl = this._parent.Routes.SelectMany( r => r.Items ).FirstOrDefault( i => i.Control.Control == controlViewModel.Control );
                if ( foundControl != null )
                {
                    foundControl.IsSelected = true;
                }
            }
        }

        public interface IMode
        {
            RouteViewModelMode Mode { get; }
            void ControlClicked( ControlViewModel controlViewModel );
        }

        #endregion Inner classes

        public void ControlClicked( ControlViewModel convert )
        {
            this.Mode.ControlClicked( convert );
        }
    }

    public enum RouteViewModelMode
    {
        ExploreRoute,
        AddItemsToRoute,
    }
}