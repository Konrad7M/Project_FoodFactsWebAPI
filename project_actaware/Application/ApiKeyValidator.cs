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
        }
    }

    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
