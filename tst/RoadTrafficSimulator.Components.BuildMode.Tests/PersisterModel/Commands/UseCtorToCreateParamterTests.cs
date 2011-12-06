using System.Linq;
using FakeItEasy;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using FluentAssertions;
using Xunit;

namespace RoadTrafficSimulator.Components.BuildMode.Tests.PersisterModel.Commands
{
    public class UseCtorToCreateParamterTests
    {
        private readonly DeserializationContext _deserializationContext = A.Fake<DeserializationContext>();

        [Fact]
        public void Should_create_object_using_default_constrcutor()
        {
            var paratmer = Actions.Ctor( () => new DefaultConstructorClass() );

            paratmer.Type.Should().Be<DefaultConstructorClass>();
            AssertionExtensions.Should( this._deserializationContext.CreateControls.First( s => s.Id == paratmer.CommandId ) ).NotBeNull();
        }

        [Fact]
        public void Should_create_object_using_serializable_paramters()
        {
            var paratmer = Actions.Ctor( () => new SerializableParamtersConstructor( Is.Const( 3 ) ) );

            var value = this._deserializationContext.CreateControls.First( s => s.Id == paratmer.CommandId );
            paratmer.Type.Should().Be<SerializableParamtersConstructor>();

            value.As<SerializableParamtersConstructor>().Value.Should().Be( 3 );
        }

        [Fact]
        public void Should_use_ioc_container_to_resolve_ioc_paramters()
        {
            var paratmer = Actions.Ctor( () => new IocParamterConsturcutor( Is.Ioc<ITestClass>() ) );

            paratmer.Type.Should().Be<IocParamterConsturcutor>();
        }
    }


    public class DefaultConstructorClass
    {
    }

    public class IocParamterConsturcutor
    {
        public ITestClass Paramter { get; set; }

        public IocParamterConsturcutor( ITestClass paramter )
        {
            this.Paramter = paramter;
        }
    }

    public class ITestClass
    {

    }

    public class SerializableParamtersConstructor
    {
        public SerializableParamtersConstructor( int paratmer )
        {
            this.Value = paratmer;
        }

        public int Value { get; set; }
    }

}