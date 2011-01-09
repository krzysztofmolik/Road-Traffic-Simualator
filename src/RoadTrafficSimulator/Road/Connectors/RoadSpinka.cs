using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Xna.Road;
using Xna.Road.RoadJoiners;
using Assert = Common.Assert;

namespace XnaVs10.Road.RoadJoiners
{
    public class RoadConnection : IRoadConnection
    {
        private readonly IList<RoadLineWrapper> _connectedRoads;
        private IList<Vector2> _shape;
        private ISubject<Unit> _shapeChanged;

        public RoadConnection( IRoadLine roadLine, RoadSide roadSide )
        {
            this._shapeChanged = new Subject<Unit>();

            var wrapper = new RoadLineWrapper( roadLine, roadSide != RoadSide.Begin );
           // this._connectedRoads = new List<RoadLineWrapper> { wrapper };
            this._connectedRoads = new List<RoadLineWrapper>();

            this._shape = this.GenerateShape();
        }

        public IObservable<Unit> ShapeChanged
        {
            get
            {
                return _shapeChanged;
            }
        }

        public void UpdateShape()
        {
            this._shape = this.GenerateShape();
            this._shapeChanged.OnNext( new Unit() );
        }

        public ReadOnlyCollection<Vector2> ShapeVectors
        {
            get { return new ReadOnlyCollection<Vector2>( this._shape ); }
        }

        public IEnumerable<IRoadLine> ConnectedRoads
        {
            get { return this._connectedRoads.Select( t => t.Orginal ); }
        }

        private IList<Vector2> GenerateShape()
        {
            if( this._connectedRoads.IsEmpty() )
            {
                return new List<Vector2>();
            }
            var sorted = this.SortClockwise( this._connectedRoads );

            var vecotrs = new List<Vector2>( sorted.Length * 2 );
            foreach ( var roadLine in sorted )
            {
                vecotrs.Add( roadLine.RightBegin );
                vecotrs.Add( roadLine.LeftBegin );
            }

            return vecotrs;
        }

        private RoadLineWrapper[] SortClockwise( IEnumerable<RoadLineWrapper> connectedRoads )
        {
            Assert.That( connectedRoads, Is.Not.Empty ).Throw<ArgumentException>();
            if ( connectedRoads.Count() == 1 )
            {
                return connectedRoads.ToArray();
            }

            var first = connectedRoads.First();
            var firstNormalized = first.EndLocation - first.BeginLocation;

            var normalized =
                connectedRoads.Skip( 1 ).Select( t =>
                                                     {
                                                         var vec = t.EndLocation - t.BeginLocation;
                                                         vec.Normalize();
                                                         return new
                                                                    {
                                                                        Wrapper = t,
                                                                        NormalizedVector = vec,
                                                                        Angel =
                                                                            Math.Acos( Vector2.Dot( firstNormalized, vec ) ),
                                                                    };
                                                     } );

            normalized.OrderBy( t => t.Angel );
 
            //throw new Exception("Sprawdzic to !!");)))
            return new[] { first }.Concat( normalized.Select( t => t.Wrapper ) ).ToArray();
        }

        public IEnumerable<Vector2> Shape
        {
            get { return this._shape; }
        }

        public void Connect( IRoadLine roadLine, RoadSide roadSide )
        {
            Assert.That( this._connectedRoads, Is.Not.Contains( roadLine ) ).Throw<ArgumentException>();

            if ( this._connectedRoads.Any( t => t.Orginal == roadLine ) )
            {
                return;
            }

            this._connectedRoads.Add( new RoadLineWrapper( roadLine, roadSide != RoadSide.Begin ));
            this._shape = this.GenerateShape();
        }

        public void Connect( RoadLineWrapper roadLine )
        {
            Assert.That( roadLine, Is.Not.Null );

            if ( this._connectedRoads.Contains( roadLine ) )
            {
                return;
            }

            this._connectedRoads.Add( roadLine );
            this._shape = this.GenerateShape();
        }

        public void Disconnect( IRoadLine roadLine )
        {
            Assert.That( this._connectedRoads, Contains.Item(roadLine)).Throw<ArgumentException>();
        }
    }
}