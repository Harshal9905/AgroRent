using Microsoft.AspNetCore.Cors.Infrastructure;

namespace AgroRent.Security
{
    public static class GlobalCorsConfig
    {
        public static void ConfigureCors(CorsPolicyBuilder builder)
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }
    }
}
