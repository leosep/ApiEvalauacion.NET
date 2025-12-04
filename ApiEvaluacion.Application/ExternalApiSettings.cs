namespace ApiEvaluacion.Application
{
    public class ExternalApiSettings
    {
        public JsonPlaceholderSettings JsonPlaceholder { get; set; } = new JsonPlaceholderSettings();
    }

    public class JsonPlaceholderSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string PostsEndpoint { get; set; } = string.Empty;

        public string PostsUrl => $"{BaseUrl}{PostsEndpoint}";
    }
}