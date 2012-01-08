using NUnit.Framework;
using RoadTrafficSimulator.Infrastructure.MathHelpers;
using FluentAssertions;

namespace RoadTrafficSimulator.Infrastructure.Tests.MathHelpers
{
    [TestFixture]
    public class MyMathHelperTests
    {
        [Test, Sequential]
        public void GetTimeToDriveThrough_start_speed(
            [Values( 0.11f, 0.21f, 0.31f )]
            float v1,
            [Values( 0.10f, 0.20f, 0.30f )]
            float v2 )
        {
            var s = 10.0f;
            var a = 1.0f;
            var vMax = 10.0f;
            var first = MyMathHelper.GetTimeToDriveThrough( s, v1, vMax, a );
            var second = MyMathHelper.GetTimeToDriveThrough( s, v2, vMax, a );

            first.Should().BeLessThan( second );
        }

        [Test, Sequential]
        public void GetTimeToDriveThrough_distance(
            [Values( 0.11f, 0.21f, 0.31f )]
            float s1,
            [Values( 0.10f, 0.20f, 0.30f )]
            float s2 )
        {
            var v = 1.0f;
            var a = 1.0f;
            var vMax = 1000.0f;
            var first = MyMathHelper.GetTimeToDriveThrough( s1, v, vMax, a );
            var second = MyMathHelper.GetTimeToDriveThrough( s2, v, vMax, a );

            first.Should().BeGreaterThan( second );
        }
    }
}