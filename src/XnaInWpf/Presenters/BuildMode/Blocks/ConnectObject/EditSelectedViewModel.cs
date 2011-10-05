using System.ComponentModel;
using Common;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Common;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Messages;
using Common.Wpf;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class EditSelectedViewModel : IBlockViewModel, IHandle<GuiCommdnEdit>, INotifyPropertyChanged
    {
        private readonly MainBlockViewModel _mainBlockViewModel;
        private readonly IEventAggregator _eventAggreator;
        private readonly NameWithIconViewModel _preview;
        private readonly ControlToControlWithRouteViewModelConveter _conveter;

        public EditSelectedViewModel( MainBlockViewModel mainBlockViewModel, IEventAggregator eventAggreator )
        {
            this._mainBlockViewModel = mainBlockViewModel;
            this._eventAggreator = eventAggreator;
            this._preview = new NameWithIconViewModel( this.Name, "" );
            this._conveter = new ControlToControlWithRouteViewModelConveter();
            this._eventAggreator.Subscribe( this );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ControlWithRoutelViewModel _controlWithRoutel;
        public ControlWithRoutelViewModel ControlWithRoutel
        {
            get { return this._controlWithRoutel; }
            set
            {
                this._controlWithRoutel = value;
                this.PropertyChanged.Raise( this, () => this.ControlWithRoutel );
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
            this._eventAggreator.Publish( this._mainBlockViewModel );
        }

        public void Execute()
        {
            this._eventAggreator.Publish( new ChangeBlock( this ) );
            this._eventAggreator.Publish( new ExecuteCommand( CommandType.SelectToEdit ) );
        }

        public void Handle( GuiCommdnEdit message )
        {
            this.ControlWithRoutel = this._conveter.Convert( message.Control );
        }
    }
}