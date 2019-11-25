![](https://github.com/KevWK314/ThreeFourteen.FluentHttpClient/workflows/BuildAndTest/badge.svg)

# ThreeFourteen.FluentHttpClient
A wrapper around HttpClient using HttpClientFactory.

## FluentHttpClient
I've found working with HttpClient is easy enough but very repetitive. Everyone doing the same thing over and over. Serealise, deserialise, add headers, add authentication. And every slight change to the flow or serialisation approach requires a new wrapper class. It's not flexible.

The actual IHttpFluentClient interface has just one method which maps to the HttpClient SendAsync method. And with the use of a builder class and extension methods we can very quickly increase the functionality of this single method.

The simplest way to create the client is by providing an already existing HttpClient.

```c#
var client = new FluentHttpClient("MyClient", new HttpClient { BaseAddress = new Uri("https://example") });
```

Then when it comes to sending a request we can use the request builder for a fluent experience.

```c#
// Get Example
var user = await client
    .Get("api/customer")
    .OnRequest(LogRequestDetails)
    .OnResponse(LogResponseDetails)
    .ExecuteAsync<User>();

// Post Examle
var createdCustomer = await client
    .Post("api/customer")
    .WithHeader("User-Agent", "Computer")
    .ExecuteAsync<CreateUserRequest, CreateUserResponse>(request);
```
