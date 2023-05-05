using Microsoft.AspNetCore.Mvc;

namespace project_actaware.MiddleWare
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthorizationFilter)) { }    
    }
}
