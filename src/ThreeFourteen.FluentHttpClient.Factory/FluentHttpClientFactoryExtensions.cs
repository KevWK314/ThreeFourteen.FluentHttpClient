using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ThreeFourteen.FluentHttpClient.Factory
{
    public static class FluentHttpClientFactoryExtensions
    {
        public static IServiceCollection AddFluentHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IFluentHttpClientFactory>(sp => new FluentHttpClientFactory(sp.GetService<IHttpClientFactory>()));

            return services;
        }

        public static IServiceCollection AddFluentHttpClient<TBuilder>(this IServiceCollection services) where TBuilder : IFluentHttpClientFactoryBuilder, new()
        {
            services.AddHttpClient();

            var builder = new TBuilder();
            builder.Build(services);
            services.AddSingleton<IFluentHttpClientFactory>(sp => new FluentHttpClientFactory(sp.GetService<IHttpClientFactory>()));

            return services;
        }

        public static IServiceCollection AddFluentHttpClient(this IServiceCollection services, IFluentHttpClientFactoryBuilder builder)
        {
            services.AddHttpClient();
            
            builder.Build(services);
            services.AddSingleton<IFluentHttpClientFactory>(sp => new FluentHttpClientFactory(sp.GetService<IHttpClientFactory>()));

            return services;
        }
    }
}