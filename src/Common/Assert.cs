using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Common
{
    public class Assert
    {
        private enum AssertResult
        {
            Passed,
            Failed,
            Unknow
        }

        public Assert()
        {
            this.IsPassed = AssertResult.Unknow;
            this.ExcpeptionType = null;
            this.MessageWriter = new TextMessageWriter();
        }

        private AssertResult IsPassed { get; set; }

        private MessageWriter MessageWriter { get; set; }

        private Type ExcpeptionType { get; set; }

        public static Assert That( object actual, IResolveConstraint expression, string message, params string[] args )
        {
            var constraint = expression.Resolve();
            var assert = new Assert();

            if (!constraint.Matches(actual))
            {
                constraint.WriteMessageTo( assert.MessageWriter );
                assert.ProcessResult( AssertResult.Failed );
            }

            assert.ProcessResult( AssertResult.Passed);

            return assert;
        }

        public static Assert That( object actual, IResolveConstraint expression )
        {
            return Assert.That( actual, expression, string.Empty );
        }

        public void Throw<TException>() where TException : Exception
        {
            this.ExcpeptionType = typeof( TException );
        }

        private void ProcessResult( AssertResult passed )
        {
            this.IsPassed = passed;
            this.ThrowIfNeed();
        }

        private void ThrowIfNeed()
        {
            if ( this.ExcpeptionType == null )
            {
                return;
            }

            if ( this.IsPassed == AssertResult.Failed )
            {
                var ctor = this.ExcpeptionType.GetConstructor( new Type[] { typeof ( string ) } );
                var exception = (Exception)ctor.Invoke( new object[] { this.MessageWriter.ToString() } );
                throw exception;
            }
        }
    }
}