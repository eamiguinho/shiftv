using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes.Stubs;

namespace Shiftv.Services.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class FakesHelper
    {
        public static T CreateNotImplementedStub<T>() where T : StubBase
        {
            var stub = Activator.CreateInstance<T>();
            stub.InstanceBehavior = StubBehaviors.NotImplemented;
            stub.InstanceObserver = new StubObserver();
            return stub;
        }
    }
}