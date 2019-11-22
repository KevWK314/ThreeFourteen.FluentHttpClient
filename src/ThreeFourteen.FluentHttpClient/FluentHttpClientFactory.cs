using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FluentHttpClient
{
    public interface IFluentHttpClientFactory
    {
        FluentHttpClient Create(string name);
        FluentHttpClient Create(string name, FluentHttpClientConfiguration configuration);
    }

    public class FluentHttpClientFactory : IFluentHttpClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FluentHttpClientFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public FluentHttpClient Create(string name)
        {
            return new FluentHttpClient(name, _httpClientFactory.CreateClient(name), null);
        }

        public FluentHttpClient Create(string name, FluentHttpClientConfiguration configuration)
        {
            return new FluentHttpClient(name, _httpClientFactory.CreateClient(name), configuration);
        }

        public static IFluentHttpClientFactory Create(IFluentHttpClientFactoryBuilder builder)
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            builder.Build(services);
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            return new FluentHttpClientFactory(httpClientFactory);
        }
    }
}