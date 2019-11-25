using Microsoft.Extensions.DependencyInjection;

namespace ThreeFourteen.FluentHttpClient.Factory
{
    public interface IFluentHttpClientFactoryBuilder
    {
        void Build(IServiceCollection services);
    }
}
