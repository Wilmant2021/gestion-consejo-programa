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
            // Simula un ID de identidad local si no se envía una cabecera específica
            return _httpContextAccessor.HttpContext?.Request.Headers["X-Simulated-User-Id"].FirstOrDefault() 
                   ?? "local-dev-user-001";
        }

        public string GetUserEmail()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers["X-Simulated-User-Email"].FirstOrDefault() 
                   ?? "usuario.prueba@universidad.edu.co";
        }
    }
}