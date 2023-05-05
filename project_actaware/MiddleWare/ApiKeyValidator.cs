namespace project_actaware.MiddleWare
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IConfiguration _configuration;
        public ApiKeyValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool IsValid(string apiKey)
        {
            return _configuration.GetValue<string>("AppSettings:ApiKey") == apiKey;
            // Implement logic for validating the API key.
        }
    }

    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
