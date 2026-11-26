using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DTOs;
using Modules.Profile.Application.Services;
using Plataforma.Modules.Profile.Infrastructure;

namespace Modules.Profile.Application.Services
{
    public class AutentificacionService
    {
        private readonly PerfilDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AutentificacionService(PerfilDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<UsuarioLoginResponseDto?> LoginAsync(LoginDto dto)
        {
            string correoSolicitado = !string.IsNullOrWhiteSpace(dto.CorreoInstitucional)
                ? dto.CorreoInstitucional.Trim().ToLowerInvariant()
                : _currentUserService.GetUserEmail().Trim().ToLowerInvariant();

            string currentIdentityProviderId = _currentUserService.GetCurrentIdentityProviderId();

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Programa)
                .FirstOrDefaultAsync(u => u.CorreoInstitucional.ToLower() == correoSolicitado);

            if (usuario == null)
            {
                return null;
            }

            if (!string.Equals(usuario.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Su cuenta se encuentra inactiva. Contacte al administrador.");
            }

            // En simulación/desarrollo, los IDs autogenerados o locales no deben bloquear el login por cabecera
            bool esIdSimulado = string.IsNullOrWhiteSpace(usuario.IdentityProviderId)
                || usuario.IdentityProviderId.StartsWith("sim-", StringComparison.OrdinalIgnoreCase)
                || usuario.IdentityProviderId.StartsWith("local-user-", StringComparison.OrdinalIgnoreCase)
                || usuario.IdentityProviderId.StartsWith("local-dev-user-", StringComparison.OrdinalIgnoreCase);

            if (!esIdSimulado
                && !string.IsNullOrWhiteSpace(currentIdentityProviderId)
                && usuario.IdentityProviderId != currentIdentityProviderId)
            {
                return null;
            }

            var nombreCompleto = $"{usuario.Nombre} {usuario.SegundoNombre ?? ""} {usuario.Apellido} {usuario.SegundoApellido ?? ""}"
                .Replace("  ", " ")
                .Trim();

            return new UsuarioLoginResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = nombreCompleto,
                CorreoInstitucional = usuario.CorreoInstitucional,
                RolesYProgramas = usuario.UsuarioRoles.Select(ur => new RolProgramaResponseDto
                {
                    RolId = ur.IdRol,
                    RolNombre = ur.Rol?.RolNombre ?? string.Empty,
                    ProgramaId = ur.IdPrograma,
                    ProgramaNombre = ur.Programa?.Nombre
                }).ToList()
            };
        }
    }
}