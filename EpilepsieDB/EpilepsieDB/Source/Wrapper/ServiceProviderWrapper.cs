using Microsoft.Extensions.DependencyInjection;
using System;

namespace EpilepsieDB.Source.Wrapper
{
    public class ServiceProviderWrapper : IServiceProviderWrapper
    {
        private readonly IServiceProvider _provider;

        public ServiceProviderWrapper(IServiceProvider provider)
        {
            _provider = provider;
        }

        public T GetService<T>()
        {
            return _provider.GetService<T>();
        }
    }
}
