using System.Linq;
using Autofac;
using FakeItEasy;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using FluentAssertions;
using Xunit;

namespace RoadTrafficSimulator.Components.BuildMode.Tests.PersisterModel.Commands
{
    public class CallActionOnCreatedObjectTests
    {
        private readonly DeserializationContext _deserializationContext = new DeserializationContext( A.Fake<ILifetimeScope>() );

        [Fact]
        public void Should_chose_correct_id()
        {
            var testClassCtor = Actions.Ctor( () => new TestClass() );
            var callAction = Actions.Call( () => testClassCtor.Default.Foo() );

            var testClass = ( TestClass ) this._deserializationContext.CreateControls.First( s => s.Id == testClassCtor.CommandId );
            callAction.Execute( this._deserializationContext );

            testClass.WasCalled.Should().BeTrue();
        }

        [Fact]
        public void Should_call_method_with_paramters()
        {
            var testClassCtor = Actions.Ctor( () => new TestClass() );
            var callAction = Actions.Call( () => testClassCtor.Default.Foo( Is.Const( 3 ) ) );

            var testClass = ( TestClass ) this._deserializationContext.CreateControls.First( s => s.Id == testClassCtor.CommandId );
            callAction.Execute( this._deserializationContext );

            testClass.WasCalledWithInt.Should().BeTrue();
        }
    }

    public class TestClass
    {
        public void Foo()
        {
            this.WasCalled = true;
        }

        public void Foo( int i )
        {
            this.WasCalledWithInt = true;
        }

        public bool WasCalledWithInt { get; private set; }

        public bool WasCalled { get; private set; }
    }
}

