namespace FluentHttpClient.Serialize
{
    public static class Serialization
    {
        public static readonly ISerialization Default = new JsonStreamSerialization();
    }
}
