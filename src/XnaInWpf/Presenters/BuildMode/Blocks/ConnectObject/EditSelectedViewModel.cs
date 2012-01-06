using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Common;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Messages;
using Common.Wpf;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class EditSelectedViewModel : IBlockViewModel, IHandle<GuiCommdnEdit>, IHandle<GuiCommandControlClicked>, INotifyPropertyChanged
    {
        private static readonly Type[] EditableControls = {
                                                               typeof( RoadTrafficSimulator.Components.BuildMode.Controls.  CarsInserter ),
                                                               typeof( RoadTrafficSimulator.Components.BuildMode.Controls.  LightBlock ),
                                                           };

        private readonly MainBlockViewModel _mainBlockViewModel;
        private readonly IEventAggregator _eventAggreator;
        private readonly NameWithIconViewModel _preview;
        private readonly ControlToControlWithRouteViewModelConveter _conveter;
        private readonly ControlToControlViewModelConveter _controlToControlWithoutRouteConveter;

        public EditSelectedViewModel( MainBlockViewModel mainBlockViewModel, IEventAggregator eventAggreator, ControlToControlViewModelConveter controlToControlWithoutRouteConveter )
        {
            this._mainBlockViewModel = mainBlockViewModel;
            this._controlToControlWithoutRouteConveter = controlToControlWithoutRouteConveter;
            this._eventAggreator = eventAggreator;
            this._preview = new NameWithIconViewModel( this.Name, "" );
            this._conveter = new ControlToControlWithRouteViewModelConveter();
            this._eventAggreator.Subscribe( this );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IControlWithRoutelViewModel _editedControl;

        private object _activeControl;
        public object ActiveControl
        {
            get { return this._activeControl; }
            set
            {
                this._activeControl = value;
                this._editedControl = value as IControlWithRoutelViewModel;
                this.PropertyChanged.Raise( this, () => this.ActiveControl );
            }
        }

        public object Preview
        {
            get { return this._preview; }
        }

        public string Name
        {
            get { return "Edit selected"; }
        }

        public void GoBack()
        {
            this.ActiveControl = null;
            this._eventAggreator.Publish( ( new ExecuteCommand( CommandType.Clear ) ) );
            this._eventAggreator.Publish( new ChangeBlock( this._mainBlockViewModel ) );
        }

        public void Execute()
        {
            this._eventAggreator.Publish( new ChangeBlock( this ) );
            this._eventAggreator.Publish( new ExecuteCommand( CommandType.SelectToEdit ) );
        }

        public void Handle( GuiCommdnEdit message )
        {
            var controls = message.Controls.Where( this.IsEditable ).Select( c => this._conveter.Convert( c ) ).ToArray();
            if ( controls.Length > 1 )
            {
                this.ChooseControl( controls );
            }
            else
            {
                this.ShowControlEdit( controls.FirstOrDefault() );
            }
        }

        private void ChooseControl( IEnumerable<IControlWithRoutelViewModel> controls )
        {
            this.ActiveControl = new ChoseControlViewModel( controls, this.ShowControlEdit );
        }

        private void ShowControlEdit( IControlWithRoutelViewModel control )
        {
            this.ActiveControl = control;
            if ( _editedControl != null )
            {
                this._eventAggreator.Publish( new ExecuteCommand( CommandType.NotifyAboutClickedControls ) );
            }
        }

        private bool IsEditable( IControl control )
        {
            if( control == null ) { return false; }
            var type = control.GetType();

            return EditableControls.Contains( type );
        }

        public void Handle( GuiCommandControlClicked message )
        {
            Debug.Assert( this.ActiveControl != null );
            if ( this._editedControl != null )
            {
                this._editedControl.ControlClicked( this._controlToControlWithoutRouteConveter.Convert( message.Control ) );
            }
        }
    }
}