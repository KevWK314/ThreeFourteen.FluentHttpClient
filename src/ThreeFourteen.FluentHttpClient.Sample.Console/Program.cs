using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace FluentHttpClient.Sample.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = FluentHttpClientFactory.Create(new ClientFactory());
            var resreqClient = factory.Create("Reqres");

            var user = resreqClient
                .Get("api/users/2")
                .OnRequest(r => LogRequestDetails(resreqClient.Name, r))
                .OnResponse(r => LogResponseDetails(resreqClient.Name, r))
                .ExecuteAsync<User>().Result;

            System.Console.WriteLine($"Get: {user}");
            System.Console.WriteLine();

            var request = new CreateUserRequest { Name = "Tim", Job = "TheBoss" };
            var createdUser = resreqClient
                .Post("api/users")
                .OnRequest(r => LogRequestDetails(resreqClient.Name, r))
                .OnResponse(r => LogResponseDetails(resreqClient.Name, r))
                .ExecuteAsync<CreateUserRequest, CreateUserResponse>(request).Result;

            System.Console.WriteLine($"Post: {createdUser}");
            System.Console.ReadKey();
        }

        static void LogRequestDetails(string name, HttpRequestMessage requestMessage)
        {
            requestMessage.Content?.LoadIntoBufferAsync().Wait();

            var contentSize = requestMessage.Content?.ReadAsByteArrayAsync().Result.Length / (double)1024 ?? 0d;
            System.Console.WriteLine($"{name} Request ({requestMessage.Method} {requestMessage.RequestUri}): content={contentSize:0.00}kb");
        }

        static void LogResponseDetails(string name, HttpResponseMessage responseMessage)
        {
            responseMessage.Content?.LoadIntoBufferAsync().Wait();
            var contentSize = responseMessage.Content?.ReadAsByteArrayAsync().Result.Length / (double)1024 ?? 0d;
            System.Console.WriteLine($"{name} Response: status={responseMessage.StatusCode}, content={contentSize:0.00}kb");
        }
    }

    public class ClientFactory : IFluentHttpClientFactoryBuilder
    {
        public void Build(IServiceCollection services)
        {
            services.AddHttpClient("Reqres", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.BaseAddress = new Uri("https://reqres.in/");
            })
            .AddTransientHttpErrorPolicy(builder =>
                builder
                    .Or<TaskCanceledException>()
                    .CircuitBreakerAsync(10, TimeSpan.FromSeconds(10)))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5)));
        }
    }
}
