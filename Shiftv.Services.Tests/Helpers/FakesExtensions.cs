using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using Microsoft.QualityTools.Testing.Fakes.Stubs;

namespace Shiftv.Services.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class FakesExtensions
    {
        public static void MethodCalled<T>(this IStub<T> stub, string methodName, int? expectedMethodCount = null) where T : class
        {
            var observer = stub.InstanceObserver as StubObserver;
            if (observer == null)
                throw new ArgumentException("No InstanceObserver installed into the stub.", "stub");

            var methodCalls = observer.GetCalls().Where(call => call.StubbedMethod.Name == methodName).ToList();
            if (!methodCalls.Any())
                throw new VerificationException(string.Format("Method {0} was expected, but was not called", methodName));

            if (!expectedMethodCount.HasValue) return;
            var methodCount = methodCalls.Count();
            if (methodCount != expectedMethodCount)
                throw new VerificationException(string.Format("Method {0} count should be {1}, but was called {2} times", methodName, expectedMethodCount, methodCount));
        }

        public static void MethodCalled<TClass, TOut>(this IStub<TClass> stub, Expression<Func<TOut>> methodNameExpression, int? expectedMethodCount = null) where TClass : class
        {
            var body = methodNameExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Expression should be a member expression", "methodNameExpression");

            var methodName = body.Member.Name;
            MethodCalled(stub, methodName, expectedMethodCount);
        }
    }
}
