![](https://github.com/KevWK314/ThreeFourteen.FluentHttpClient/workflows/BuildAndTest/badge.svg)

# FluentHttpClient
A comfy wrapper around HttpClient and HttpClientFactory.

## FluentHttpClient
I've found working with HttpClient is easy enough but very repetitive. Everyone doing the same thing over and over. Serealise, deserialise, add headers, add authentication. And every slight change to the flow or serialisation approach requires a new wrapper class. It's not flexible.

The actual IHttpFluentClient interface has just one method which maps to the HttpClient SendAsync method. And with the use of a builder class and extension methods we can very quickly increase the functionality of this single method.

### Getting Started

The simplest way to create the client is by providing an already existing HttpClient.

```c#
var client = new FluentHttpClient("MyClient", new HttpClient { BaseAddress = new Uri("https://example") });
```

Then when it comes to sending a request we can use the request builder for a fluent experience.

```c#
// GET Example
var user = await client
    .Get("api/user")
    .OnRequest(LogRequestDetails)
    .OnResponse(LogResponseDetails)
    .ExecuteAsync<User>();

// POST Exaple
var createdUser = await client
    .Post("api/user")
    .WithHeader("User-Agent", "Computer")
    .Configure(c => c.HttpCompletionOption = HttpCompletionOption.ResponseHeadersRead)
    .ExecuteAsync<CreateUserRequest, CreateUserResponse>(request);
```

### Configuration

It's possible to specify basic configuration when creating the client (in the constructor) or you can specify when building a request (the request config will overide the client config). It's relatively basic configuration but allows for further flexibility.

- Serialization - The default is json serialisation. It's also the only implementation but it should be easy enough to roll your own to support whatever you require (bson, protobuf, xml, the world is your oyster).
- EnsureSuccessStatusCode - The default is true. [This will force an exception](https://docs.microsoft.com/en-us/uwp/api/windows.web.http.httpresponsemessage.ensuresuccessstatuscode) to be thrown when the response is not a success (in the 200 range).
- HttpCompletionOption - Used when sending request and [indicates when the response is complete](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcompletionoption).

### Message Listeners

It's possle to intercept both the [HttpRequestMessage](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httprequestmessage) and [HttpResponseMessage](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpresponsemessage) by injecting a message listener to either the client (which will be used for all requests using the client) or to the request being built (only used once).

```c#
// Add message listener to the fluent client
var listener = new MessageListener();
var client = new FluentHttpClient("MyClient", httpClient)
    .WithListener(listener);

// Add message listener to the request
var response = await client
    .Get("url")
    .WithListener<MessageListener>()
    .ExecuteAsync<Person>();
```

The message listener could be used for a number of handy actions:
- Log request and/or response (content, content size, timings)
- Update request (headers, authentication). 
- Custom error handling (i.e. throw custom exception based on response status code)

## FluentHttpClientFactory

FluentHttpClientFactory is a wrapper around [HttpClientFactory](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests) and will return a FluentHttpClient. It doesn't have to be used to use FluentHttpClient but it can help remove (the admittedly not very large) complexity around HttpClient/HttpClientFactory.

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

The [AddHttpClient](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.httpclientfactoryservicecollectionextensions.addhttpclient) method will allow you to pre-create an HttpClient and configure it, ready for use.

The AddTransientHttpErrorPolicy will allow you to define how failures are handled. 

### Polly

[Polly](https://github.com/App-vNext/Polly) is a .NET resilience and transient-fault-handling library. I would definitely recommend using Polly if you're writing a commercial service/application and even more so if you're writing microservices. Something like [Circuit Breakers](https://martinfowler.com/bliki/CircuitBreaker.html) can be invaluable.

## P.S. 

This is very much a work in progress. Please feel free to put forward any changes you think will add value. If you find yourself repeating the same thing over and over, it might just belong here.
