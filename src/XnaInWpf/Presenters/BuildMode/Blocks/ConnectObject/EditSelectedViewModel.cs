using System.ComponentModel;
using System.Diagnostics;
using Common;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Common;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Messages;
using Common.Wpf;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class EditSelectedViewModel : IBlockViewModel, IHandle<GuiCommdnEdit>, IHandle<GuiCommandControlClicked>, INotifyPropertyChanged
    {
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

        private ControlWithRoutelViewModel _controlWithRoute;
        public ControlWithRoutelViewModel ControlWithRoute
        {
            get { return this._controlWithRoute; }
            set
            {
                this._controlWithRoute = value;
                this.PropertyChanged.Raise( this, () => this.ControlWithRoute );
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
            this.ControlWithRoute = null;
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
            this.ControlWithRoute = this._conveter.Convert( message.Control );
            this._eventAggreator.Publish( new ExecuteCommand( CommandType.NotifyAboutClickedControls ) );
        }

        public void Handle( GuiCommandControlClicked message )
        {
            Debug.Assert( this.ControlWithRoute != null );
            this.ControlWithRoute.ControlClicked( this._controlToControlWithoutRouteConveter.Convert( message.Control ) );
        }
    }
}