using Microsoft.AspNetCore.Http;
using Modules.Profile.Application.Services;

namespace Plataforma.Modules.Profile.Infrastructure
{
    public class LocalDevCurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalDevCurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentIdentityProviderId()
        {
            var headers = _httpContextAccessor.HttpContext?.Request.Headers;
            if (headers != null && headers.TryGetValue("X-Simulated-User-Id", out var value))
            {
                var trimmed = value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    return trimmed;
                }
            }

            return "local-dev-user-001";
        }

        public string GetUserEmail()
        {
            var headers = _httpContextAccessor.HttpContext?.Request.Headers;
            if (headers != null && headers.TryGetValue("X-Simulated-User-Email", out var value))
            {
                var trimmed = value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    return trimmed;
                }
            }

            return "usuario.prueba@universidad.edu.co";
        }
    }
}