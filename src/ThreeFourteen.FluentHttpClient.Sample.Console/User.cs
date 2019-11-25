using System;
using Newtonsoft.Json;

namespace ThreeFourteen.FluentHttpClient.Sample.Console
{
    public class User
    {
        public Data Data { get; set; }

        public override string ToString()
        {
            return $"{Data.Id} - {Data.FirstName} {Data.LastName}";
        }
    }

    public class Data
    {
        public int Id { get; set; }
        public string Email { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("avatar")]
        public string AvatarUri { get; set; }
    }

    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Job { get; set; }
    }

    public class CreateUserResponse
    {
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("job")]
        public string Job { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name} is {Job}";
        }
    }
}
