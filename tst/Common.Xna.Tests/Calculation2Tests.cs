using Microsoft.Xna.Framework;
using NUnit.Framework;
using VectorOption = Microsoft.FSharp.Core.FSharpOption<Microsoft.Xna.Framework.Vector2>;

namespace Common.Xna.Tests
{
    [TestFixture]
    public class Calculation2Tests
    {
        private const float RoadHeight = 2.0f;
        private Calculation2 _calculationAroundStartPoint;
        private Calculation2 _calculationAroundEndPoint;

        [SetUp]
        public void SetUp()
        {
            this._calculationAroundStartPoint = new Calculation2( PointRotation.Start, RoadHeight );
            this._calculationAroundEndPoint = new Calculation2( PointRotation.End, RoadHeight );
        }

        #region StartPoint

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );
            var next = new Vector2( 5.0f, 5.0f );
            var result = this._calculationAroundStartPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 3.0f, 2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane2()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );
            var next = new Vector2( 5.0f, -5.0f );
            var result = this._calculationAroundStartPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 7.0f, 2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane3()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );
            var next = new Vector2( -5.0f, -5.0f );
            var result = this._calculationAroundStartPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -3.0f, -2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane4()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );
            var next = new Vector2( -5.0f, 5.0f );
            var result = this._calculationAroundStartPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -7.0f, -2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );

            var result = this._calculationAroundStartPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( 5, 2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector2()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 0.0f, -5.0f );

            var result = this._calculationAroundStartPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( 2, -5 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector3()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );

            var result = this._calculationAroundStartPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( -5, -2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector4()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -0.0f, 5.0f );

            var result = this._calculationAroundStartPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( -2, 5 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 5.0f, 0.0f );

            var result = this._calculationAroundStartPoint.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 0, 2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector2()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 0.0f, -5.0f );

            var result = this._calculationAroundStartPoint.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 2, 0 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector3()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( -5.0f, 0.0f );

            var result = this._calculationAroundStartPoint.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 0, -2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector4()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 0.0f, 5.0f );

            var result = this._calculationAroundStartPoint.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -2, 0 ) ) );
        }

        #endregion StartPoint

        #region EndPoint

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane_endPoint()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );
            var next = new Vector2( 5.0f, 5.0f );
            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 7.0f, -2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane2_endPoint()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );
            var next = new Vector2( 5.0f, -5.0f );
            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 3.0f, -2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane3_endPoint()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );
            var next = new Vector2( -5.0f, -5.0f );
            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 7.0f, 2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane4_endPoint()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );
            var next = new Vector2( -5.0f, 5.0f );
            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -3.0f, 2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane4_endPoint2()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );
            var next = new Vector2( -5.0f, 5.0f );
            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -3.0f, 2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector_endPoint()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );

            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( 5, 2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector2_endPoint()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 0.0f, -5.0f );

            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( 2, -5 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector3_endPoint()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );

            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( -5, -2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector4_endPoitn()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -0.0f, 5.0f );

            var result = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( -2, 5 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector_endPoint()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 5.0f, 0.0f );

            var result = this._calculationAroundEndPoint.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 0, 2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector2_endPoint()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 0.0f, -5.0f );

            var result = this._calculationAroundEndPoint.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 2, 0 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector3_endPoint()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( -5.0f, 0.0f );

            var result = this._calculationAroundEndPoint.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 0, -2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector4_endPoint()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 0.0f, 5.0f );

            var result = this._calculationAroundEndPoint.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -2, 0 ) ) );
        }

        [Test]
        public void Test1()
        {
            var orginalStart = new Vector2( 0.0f, 0.0f );
            var orginalEnd = new Vector2( -2.0f, 2.0f );
            var prevStart = new Vector2( -4.0f, 0.0f );
            var prevEnd = new Vector2( -4.0f, 2.0f );
            var nextStart = new Vector2( 2.0f, 4.0f );
            var nextEnd = new Vector2( 0.0f, 4.0f );

            var resultAroundStart = this._calculationAroundEndPoint.Calculate( VectorOption.Some( prevEnd ), orginalEnd, VectorOption.Some( nextEnd ) );
            var recalculate = new CalculateEdgeAngel( RoadHeight ).Calculate(
                VectorOption.Some( this.Location( prevStart, prevEnd ) ),
                this.Location( resultAroundStart, orginalEnd ),
                VectorOption.Some( this.Location( nextStart, nextEnd ) ) );

            Assert.That( resultAroundStart, Is.EqualTo( recalculate.Start ) );

        }

        private Vector2 Location( Vector2 start, Vector2 end )
        {
            return start + ( ( end - start ) / 2 );
        }

        #endregion StartPoint

    }
}