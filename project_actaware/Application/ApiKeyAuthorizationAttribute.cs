using Microsoft.AspNetCore.Mvc;

namespace project_actaware.MiddleWare
{
    public class ApiKeyAuthorizationAttribute : ServiceFilterAttribute
    {
        public ApiKeyAuthorizationAttribute() : base(typeof(ApiKeyAuthorizationFilter)) { }    
    }
}
