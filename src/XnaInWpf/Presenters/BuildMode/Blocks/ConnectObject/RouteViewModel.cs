﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Caliburn.Micro;
using Common.Wpf;
using System.Linq;
using Microsoft.Xna.Framework;
using Common;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class RouteViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<RouteItemViewModel> _items;
        private readonly Route _orignalRoute;
        private bool _isAddMode;

        public RouteViewModel( string name, Route orignalRoute )
        {
            this._items = new ObservableCollection<RouteItemViewModel>();
            this.Name = name;
            this._orignalRoute = orignalRoute;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;

        public string Name
        {
            get { return this._name; }
            set { this._name = value; this.PropertyChanged.Raise( this, () => this.Name ); }
        }

        public void Add( RouteItemViewModel item )
        {
            this._items.Add( item );
        }

        public void Remove( object item )
        {
            var element = item as FrameworkElement;
            if ( element == null )
            {
                return;
            }
            this._items.Remove( ( RouteItemViewModel ) element.DataContext );
        }

        public ObservableCollection<RouteItemViewModel> Items
        {
            get { return this._items; }
        }

        public void SwitchToAddMode()
        {
            this.IsAddMode = true;
        }

        public void SwitchToSelectionMode()
        {
        }

        protected bool IsAddMode
        {
            get
            {
                return this._isAddMode;
            }
            set
            {
                this._isAddMode = value;
                this.PropertyChanged.Raise( this, () => this.IsAddMode );
            }
        }

        public void ControlClicked( ControlViewModel controlViewModel )
        {
            if ( this.IsAddMode )
            {
                this.AddToList( controlViewModel );
            }
            else
            {
                this.SelectControl( controlViewModel );
            }
        }

        private void AddToList( ControlViewModel controlViewModel )
        {
            var canAdd = this._orignalRoute.CanAdd( controlViewModel.Control );
            if ( !canAdd ) { return; }

            var routeElement = new RouteElement( controlViewModel.Control, PriorityType.None );
            this._orignalRoute.Add( routeElement );
            var priorities = this._orignalRoute.GetPrioritiesFor( controlViewModel.Control );
            Execute.OnUIThread( () => this.Items.Add( new RouteItemViewModel( controlViewModel, priorities, routeElement ) ) );
        }

        private void SelectControl( ControlViewModel controlViewModel )
        {
            this.Items.ForEach( f => f.IsSelectedOnSimulator = false );
            var controlToSelect = this.Items.FirstOrDefault( f => f.Control.Control.Id == controlViewModel.Control.Id );
            if ( controlToSelect == null ) { return; }

            controlToSelect.Control.Control.VertexContainer.Color = Color.Azure;
            controlToSelect.IsSelectedOnSimulator = true;
        }
    }
}