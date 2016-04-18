namespace Shiftv.Global
{
    public static class IocContainer
    {
        public static IIoc Current = new MyIoc();

        public static T Resolve<T>()
        {
            return Current.Resolve<T>();
        }

        public static void RegisterType<TInterface, TClass>()
            where TClass : TInterface
        {
            Current.RegisterType<TInterface, TClass>();
        }

        public static void RegisterSingleton<TInterface, TClass>()
            where TClass : TInterface, new()
        {
            Current.RegisterSingleton<TInterface, TClass>();
        }
    }

    public class MyIoc : IocLight
    {
        protected override void InitializeContainer()
        {

        }
    }
}