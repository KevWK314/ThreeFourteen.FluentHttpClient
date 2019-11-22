using Microsoft.Extensions.DependencyInjection;

namespace FluentHttpClient
{
    public interface IFluentHttpClientFactoryBuilder
    {
        void Build(IServiceCollection services);
    }
}
