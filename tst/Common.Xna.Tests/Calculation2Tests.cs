using Microsoft.Xna.Framework;
using NUnit.Framework;
using VectorOption = Microsoft.FSharp.Core.FSharpOption<Microsoft.Xna.Framework.Vector2>;

namespace Common.Xna.Tests
{
    [TestFixture]
    public class Calculation2Tests
    {
        private const float RoadHeight = 2.0f;
        private Calculation2 _calculation2;

        [SetUp]
        public void SetUp()
        {
            this._calculation2 = new Calculation2( PointRotation.Start, RoadHeight );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );
            var next = new Vector2( 5.0f, 5.0f );
            var result = this._calculation2.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 3.0f, 2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane2()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );
            var next = new Vector2( 5.0f, -5.0f );
            var result = this._calculation2.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 7.0f, 2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane3()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );
            var next = new Vector2( -5.0f, -5.0f );
            var result = this._calculation2.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -3.0f, -2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculate_points_for_parpendicular_road_lane4()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );
            var next = new Vector2( -5.0f, 5.0f );
            var result = this._calculation2.Calculate( VectorOption.Some( prev ), orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -7.0f, -2.0f ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 5.0f, 0.0f );

            var result = this._calculation2.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( 5, 2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector2()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( 0.0f, -5.0f );

            var result = this._calculation2.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( 2, -5 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector3()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -5.0f, 0.0f );

            var result = this._calculation2.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( -5, -2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_prev_vector4()
        {
            var prev = new Vector2( 0.0f, 0.0f );
            var orginal = new Vector2( -0.0f, 5.0f );

            var result = this._calculation2.Calculate( VectorOption.Some( prev ), orginal, VectorOption.None );

            Assert.That( result, Is.EqualTo( new Vector2( -2, 5 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 5.0f, 0.0f );

            var result = this._calculation2.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 0, 2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector2()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 0.0f, -5.0f );

            var result = this._calculation2.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 2, 0 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector3()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( -5.0f, 0.0f );

            var result = this._calculation2.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( 0, -2 ) ) );
        }

        [Test]
        public void Should_correct_calculte_points_for_next_vector4()
        {
            var orginal = new Vector2( 0.0f, 0.0f );
            var next = new Vector2( 0.0f, 5.0f );

            var result = this._calculation2.Calculate( VectorOption.None, orginal, VectorOption.Some( next ) );

            Assert.That( result, Is.EqualTo( new Vector2( -2, 0 ) ) );
        }
    }
}