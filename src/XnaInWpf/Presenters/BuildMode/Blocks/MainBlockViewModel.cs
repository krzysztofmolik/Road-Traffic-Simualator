using System.Collections.ObjectModel;
using Common;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.CarsInserter;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.CarsRemover;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Junctions;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Lights;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.RoadLane;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks
{
    public class MainBlockViewModel : IBlockViewModel
    {
        private readonly ObservableCollection<IBlockViewModel> _blocks;
        private readonly IEventAggregator _eventAggreator;

        public MainBlockViewModel( IEventAggregator eventAggreator )
        {
            this._eventAggreator = eventAggreator;
            this._blocks = new ObservableCollection<IBlockViewModel>
                ( new IBlockViewModel[]  {
                                             new RoadJunctionBlockViewModelModel( this, this._eventAggreator ),
                                             new OneRoadLaneViewModel( this, this._eventAggreator ),
                                             new CarsInserterViewModel( this, this._eventAggreator ),
                                             new CarsRemoverViewModel( this, this._eventAggreator ),
                                             new StandardLightViewModel( this, this._eventAggreator ),
                                             new ConnectObjectViewModel( this, this._eventAggreator ),
                                             new EditSelectedViewModel( this, this._eventAggreator, new ControlToControlViewModelConveter() ),
                                         } );
        }

        public object Preview
        {
            get { return null; }
        }

        public string Name
        {
            get { return string.Empty; }
        }

        public void GoBack()
        {
        }

        public void Execute()
        {
        }

        public void Execute( IBlockViewModel blockViewModel )
        {
            if ( blockViewModel != null )
            {
                blockViewModel.Execute();
            }
        }

        public ObservableCollection<IBlockViewModel> Blocks { get { return this._blocks; } }
    }
}