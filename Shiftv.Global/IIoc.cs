namespace Shiftv.Global
{
    public interface IIoc
    {
        T Resolve<T>();
        void RegisterType<TInterface, TClass>() where TClass : TInterface;
        void RegisterSingleton<TInterface, TClass>() where TClass : TInterface, new();
    }
}
