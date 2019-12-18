![](https://github.com/KevWK314/ThreeFourteen.FluentHttpClient/workflows/BuildAndTest/badge.svg)

# FluentHttpClient
A comfy wrapper around HttpClient and HttpClientFactory.

## FluentHttpClient
I've found working with HttpClient is easy enough but very repetitive. Everyone doing the same thing over and over. Serealise, deserialise, add headers, add authentication.

This library is a simple wrapper around HttpClient that will hopefully make life easier and reduce duplication.

### Create a Client

The simplest way to create the client is by providing an already existing HttpClient.

```c#
var client = new FluentHttpClient("MyClient", new HttpClient { BaseAddress = new Uri("https://example") });
```
Alternatively, there is a FluentHttpClientBuilder if you are so inclined.

```c#
var httpClient = new HttpClient { BaseAddress = new Uri("https://example") };
var client = new FluentHttpClientBuilder("BuilderExample", httpClient)
    .Configure(o => o.HttpCompletionOption = HttpCompletionOption.ResponseHeadersRead)
    .OnRequest(r => r.Headers.Add("User-Agent", "Computer"))
    .Build();
```

### Use a Client

Then when it comes to sending a request we can use the request builder for a fluent experience.

```c#
// GET Example
var getResponse = await client
    .Get("api/users/2")
    .OnRequest(r => LogRequestDetails(client.Name, r))
    .OnResponse(r => LogResponseDetails(client.Name, r))
    .ExecuteAsync<User>();

// POST Exaple
var postResponse = await client
    .Post("api/users")
    .WithHeader("User-Agent", "TheComputer")
    .Configure(c => c.Serialization = new JsonSerialization(settings))
    .ExecuteAsync<CreateUserRequest, CreateUserResponse>(request);
```

### Options

It's possible to specify basic options when creating or using the client or creating a request (using the Configure method). It's relatively basic but allows for further flexibility.

- Serialization - The default is json serialisation. It's also the only implementation but it should be easy enough to roll your own to support whatever you require (bson, protobuf, xml, the world is your oyster).
- EnsureSuccessStatusCode - The default is true. [This will force an exception](https://docs.microsoft.com/en-us/uwp/api/windows.web.http.httpresponsemessage.ensuresuccessstatuscode) to be thrown when the response is not a success (in the 200 range).
- HttpCompletionOption - Used when sending request and [indicates when the response is complete](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcompletionoption).

### Message Listeners

It's possible to intercept both the [HttpRequestMessage](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httprequestmessage) and [HttpResponseMessage](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpresponsemessage) by injecting a message listener to either the client (which will be used for all requests using the client) or to the request being built (only used once).

```c#
// Add message listener to the fluent client
var listener = new MessageListener();
var client = new FluentHttpClient("Test", httpClientTester.Client, listener);

// Add message listener to the request
var response = await client
    .Get("url")
    .WithListener<MessageListener>()
    .ExecuteAsync<Person>();
```

The message listener could be used for a number of things:
- Log request and/or response (content, content size, timings)
- Update request (headers, authentication). 
- Custom error handling (i.e. throw custom exception based on response status code)

## FluentHttpClientFactory

FluentHttpClientFactory is a wrapper around [HttpClientFactory](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests) and will return a FluentHttpClient. It doesn't have to be used to use FluentHttpClient but it can help remove (the admittedly not very large) complexity around HttpClient/HttpClientFactory.

```c#
var factory = FluentHttpClientFactory.Create(new ClientFactory());
var client = factory.CreateClient("NewClient",
    b => b.Configure(r => r.EnsureSuccessStatusCode = false)
        .AddMessageListener<MessageListener>());
```

In the example above the CreateClient call will get the HttpClient from ClientFactory with the name of "NewClient". Ideally you would have pre-configured the HttpClient "NewClient".

## AspNetCore

There are extension methods in the Factory library that will allow you to add IFluentHttpClientFactory. It will be registered as a singleton service and can be injected wherever you need it. It should be called in Startup ConfigureServices.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddFluentHttpClient<ClientFactory>()
        ...
}
```

### Factory Builder

The Factory does allow for a "builder" to be injected. The factory will then provide a [ServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollection) which when used in conjunction with some handy Microsoft (and other) extension methods can build your clients, ready for use.

Here is a basic example with just one HttpClient.
```c#
    public class ClientFactory : IFluentHttpClientFactoryBuilder
    {
        public void Build(IServiceCollection services)
        {
            services.AddHttpClient("Reqres", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.BaseAddress = new Uri("https://service.io/");
            })
            .AddTransientHttpErrorPolicy(builder =>
                builder
                    .Or<TaskCanceledException>()
                    .CircuitBreakerAsync(10, TimeSpan.FromSeconds(10)))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5)));
        }
    }
```

The [AddHttpClient](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.httpclientfactoryservicecollectionextensions.addhttpclient) method will allow you to pre-create an HttpClient and configure it (BaseAddress, Timeout, etc), ready for use.

The AddTransientHttpErrorPolicy will allow you to define how failures are handled. 

### Polly

[Polly](https://github.com/App-vNext/Polly) is a .NET resilience and transient-fault-handling library. I would definitely recommend using Polly if you're writing a commercial service/application and even more so if you're writing microservices. Something like [Circuit Breakers](https://martinfowler.com/bliki/CircuitBreaker.html) can be invaluable.

## P.S. 

This is very much a work in progress. Please feel free to put forward any changes you think will add value. If you find yourself repeating the same thing over and over, it might just belong here.
