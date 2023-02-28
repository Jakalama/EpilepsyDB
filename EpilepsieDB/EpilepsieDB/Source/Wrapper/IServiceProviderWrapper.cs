namespace EpilepsieDB.Source.Wrapper
{
    public interface IServiceProviderWrapper
    {
        T GetService<T>();
    }
}
