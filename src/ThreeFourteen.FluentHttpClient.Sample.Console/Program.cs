﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using ThreeFourteen.FluentHttpClient.Factory;

namespace ThreeFourteen.FluentHttpClient.Sample.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TryItOut().Wait();

            System.Console.ReadKey();
        }

        static async Task TryItOut()
        {
            var factory = FluentHttpClientFactory.Create(new ClientFactory());
            var client = factory.Create("Reqres");

            var getResponse = await client
                .Get("api/users/2")
                .OnRequest(r => LogRequestDetails(client.Name, r))
                .OnResponse(r => LogResponseDetails(client.Name, r))
                .ExecuteAsync<User>();

            System.Console.WriteLine($"Get: {getResponse.Result}");
            System.Console.WriteLine();

            var request = new CreateUserRequest { Name = "Tim", Job = "TheBoss" };
            var postResponse = await client
                .Post("api/users")
                .OnRequest(r => LogRequestDetails(client.Name, r))
                .OnResponse(r => LogResponseDetails(client.Name, r))
                .ExecuteAsync<CreateUserRequest, CreateUserResponse>(request);

            System.Console.WriteLine($"Post: {postResponse.Result}");
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
