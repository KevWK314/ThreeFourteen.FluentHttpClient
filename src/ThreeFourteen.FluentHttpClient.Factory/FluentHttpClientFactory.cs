using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ThreeFourteen.FluentHttpClient.Factory
{
    public interface IFluentHttpClientFactory
    {
        FluentHttpClient CreateClient(string name);
        FluentHttpClient CreateClient(string name, Action<FluentHttpClientBuilder> build);
    }

    public class FluentHttpClientFactory : IFluentHttpClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FluentHttpClientFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public FluentHttpClient CreateClient(string name)
        {
            return CreateClient(name, null);
        }

        public FluentHttpClient CreateClient(string name, Action<FluentHttpClientBuilder> build)
        {
            var builder = new FluentHttpClientBuilder(name, _httpClientFactory.CreateClient(name));
            
            build?.Invoke(builder);

            return builder.Build();
        }

        public static IFluentHttpClientFactory Create<TBuilder>() where TBuilder : IFluentHttpClientFactoryBuilder, new()
        {
            var builder = new TBuilder();
            return Create(builder);
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