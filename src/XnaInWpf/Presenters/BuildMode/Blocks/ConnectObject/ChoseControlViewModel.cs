using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ChoseControlViewModel
    {
        private readonly Action<IControlWithRoutelViewModel> _showControlEdit;
        private readonly ObservableCollection<IControlWithRoutelViewModel> _controls;

        public ChoseControlViewModel( IEnumerable<IControlWithRoutelViewModel> controls, Action<IControlWithRoutelViewModel> showControlEdit )
        {
            this._showControlEdit = showControlEdit;
            this._controls = new ObservableCollection<IControlWithRoutelViewModel>( controls );
        }

        public ObservableCollection<IControlWithRoutelViewModel> Controls
        {
            get { return this._controls; }
        }

        public void Choose( RoutedEventArgs arg )
        {
            var frameWorkElement = arg.Source as FrameworkElement;

            if ( frameWorkElement == null ) { return; }
            var control = frameWorkElement.DataContext as IControlWithRoutelViewModel;

            if ( control == null ) { return; }
            this._showControlEdit( control );
        }

        public void Cancel()
        {
            this._showControlEdit( null );
        }
    }
}