using Microsoft.FSharp.Core;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Common.Xna.Tests
{
    [TestFixture]
    public class CalculateEdgeAngelTests
    {
        private CalculateEdgeAngel _calculator;

        [SetUp]
        public void SetUp()
        {
            this._calculator = new CalculateEdgeAngel( roadHeight: 2.0f );
        }

        [Test]
        public void Calculate_use_prev_value()
        {
            var prevLocation = FSharpOption<Vector2>.Some( new Vector2( 0.0f, 0.0f ) );
            var baseLocaiton = new Vector2( 1.0f, 0.0f );
            var nextLocation = FSharpOption<Vector2>.None;

            var result = this._calculator.Calculate( prevLocation, baseLocaiton, nextLocation );

            Assert.That( result.Start, Is.EqualTo( new Vector2( 1, -1 ) ) );
            Assert.That( result.End, Is.EqualTo( new Vector2( 1, 1 ) ) );
        }

        [Test]
        public void Calculate_use_next_value()
        {
            var baseLocaiton = new Vector2( 0.0f, 0.0f );
            var prevLocation = FSharpOption<Vector2>.None;
            var nextLocation = FSharpOption<Vector2>.Some( new Vector2( 1.0f, 0.0f ) );

            var result = this._calculator.Calculate( prevLocation, baseLocaiton, nextLocation );

            Assert.That( result.Start, Is.EqualTo( new Vector2( 0, -1 ) ) );
            Assert.That( result.End, Is.EqualTo( new Vector2( 0, 1 ) ) );
        }

        [Test]
        public void Calculte_use_prev_and_next_value()
        {
            var baseLocaiton = new Vector2( 0.0f, 0.0f );
            var prevLocation = FSharpOption<Vector2>.Some( new Vector2( -1, 0 ) );
            var nextLocation = FSharpOption<Vector2>.Some( new Vector2( 1.0f, 0.0f ) );

            var result = this._calculator.Calculate( prevLocation, baseLocaiton, nextLocation );

            Assert.That( result.Start, Is.EqualTo( new Vector2( 0, -1 ) ) );
            Assert.That( result.End, Is.EqualTo( new Vector2( 0, 1 ) ) );
        }

        [Test]
        public void Calculte_use_prev_and_next_value2()
        {
            var baseLocaiton = new Vector2( 0.0f, 0.0f );
            var prevLocation = FSharpOption<Vector2>.Some( new Vector2( -5, 0 ) );
            var nextLocation = FSharpOption<Vector2>.Some( new Vector2( 0.0f, -5.0f ) );

            var result = this._calculator.Calculate( prevLocation, baseLocaiton, nextLocation );

            Assert.That( result.Start, Is.EqualTo( new Vector2( -1, -1 ) ) );
            Assert.That( result.End, Is.EqualTo( new Vector2( 1, 1 ) ) );
        }
    }
}