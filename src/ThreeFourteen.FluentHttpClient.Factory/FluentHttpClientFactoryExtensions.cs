using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ThreeFourteen.FluentHttpClient.Factory
{
    public static class FluentHttpClientFactoryExtensions
    {
        public static ServiceCollection AddFluentHttpClient(this ServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IFluentHttpClientFactory>(sp => new FluentHttpClientFactory(sp.GetService<IHttpClientFactory>()));

            return services;
        }

        public static ServiceCollection AddFluentHttpClient<TBuilder>(this ServiceCollection services) where TBuilder : IFluentHttpClientFactoryBuilder, new()
        {
            services.AddHttpClient();

            var builder = new TBuilder();
            builder.Build(services);
            services.AddSingleton<IFluentHttpClientFactory>(sp => new FluentHttpClientFactory(sp.GetService<IHttpClientFactory>()));

            return services;
        }

        public static ServiceCollection AddFluentHttpClient(this ServiceCollection services, IFluentHttpClientFactoryBuilder builder)
        {
            services.AddHttpClient();
            
            builder.Build(services);
            services.AddSingleton<IFluentHttpClientFactory>(sp => new FluentHttpClientFactory(sp.GetService<IHttpClientFactory>()));

            return services;
        }
    }
}