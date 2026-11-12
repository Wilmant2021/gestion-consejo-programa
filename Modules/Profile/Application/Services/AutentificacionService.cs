using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DOTs;
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
            // 1. Obtenemos el identity_provider_id simulado actual (ej. de la cabecera HTTP)
            string currentIdentityProviderId = _currentUserService.GetCurrentIdentityProviderId();

            // 2. Buscar al usuario validando el correo, el IdentityProviderId y que su Estado sea estrictamente "Activo"
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Programa)
                .FirstOrDefaultAsync(u => u.CorreoInstitucional == dto.CorreoInstitucional 
                                       && u.IdentityProviderId == currentIdentityProviderId 
                                       && u.Estado == "Activo");

            if (usuario == null)
            {
                // Opcional: Validar si el usuario existe pero su cuenta está inactiva para dar un mensaje específico
                var usuarioInactivo = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.CorreoInstitucional == dto.CorreoInstitucional 
                                           && u.IdentityProviderId == currentIdentityProviderId 
                                           && u.Estado == "Inactivo");

                if (usuarioInactivo != null)
                {
                    throw new UnauthorizedAccessException("Su cuenta se encuentra inactiva. Contacte al administrador.");
                }

                return null; // Usuario no encontrado, no registrado o la identidad simulada no coincide
            }

            // 3. Mapear los datos al DTO de respuesta para el frontend
            var nombreCompleto = $"{usuario.Nombre} {usuario.SegundoNombre ?? ""} {usuario.Apellido} {usuario.SegundoApellido ?? ""}"
                .Replace("  ", " ")
                .Trim();

            var response = new UsuarioLoginResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = nombreCompleto,
                CorreoInstitucional = usuario.CorreoInstitucional,
                RolesYProgramas = usuario.UsuarioRoles.Select(ur => new RolProgramaResponseDto
                {
                    RolId = ur.IdRol,
                    RolNombre = ur.Rol?.RolNombre ?? string.Empty,
                    ProgramaId = ur.IdPrograma,
                    ProgramaNombre = ur.Programa?.Nombre // Si es null (rol global), quedará como null
                }).ToList()
            };

            return response;
        }
    }
}