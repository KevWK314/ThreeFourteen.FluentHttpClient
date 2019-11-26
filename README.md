![](https://github.com/KevWK314/ThreeFourteen.FluentHttpClient/workflows/BuildAndTest/badge.svg)

# ThreeFourteen FluentHttpClient
A comfy wrapper around HttpClient using HttpClientFactory.

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
    .ExecuteAsync<CreateUserRequest, CreateUserResponse>(request);
```

### Configuration

It's possible to specify basic configuration when creating the client (in the constructor) or you can specify when building a request (the request config will overide the client config). It's relatively basic configuration but allows for further flexibility.

- Serialization - The default is json serialisation. It's also the only implementation but it should be easy enough to roll your own to support whatever you require (bson, protobuf, xml, the world is your oyster).
- EnsureSuccessStatusCode - The default is true. [This will force an exception](https://docs.microsoft.com/en-us/uwp/api/windows.web.http.httpresponsemessage.ensuresuccessstatuscode) to be thrown when the response is not a success (in the 200 range).
- HttpCompletionOption - Used when sending request and [indicates when the response is complete](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcompletionoption).
- CancellationToken - Default is none. In fairness this has less value at the client level and more at the request level.

## P.S. 

This is very much a work in progress. Please feel free to put forward any changes you think will add value. If you find yourself repeating the same thing over and over, it might just belong here.

