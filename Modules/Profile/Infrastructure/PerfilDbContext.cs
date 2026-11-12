using Microsoft.EntityFrameworkCore;
using Modules.Profile.Domain;

namespace Plataforma.Modules.Profile.Infrastructure
{
    public class PerfilDbContext : DbContext
    {
        public PerfilDbContext(DbContextOptions<PerfilDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Programa> Programas { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<EstudiantePrograma> EstudianteProgramas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Definir esquema para el Monolito Modular
            modelBuilder.HasDefaultSchema("perfil");

            // Configuración de la tabla Rol
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");
                entity.HasKey(e => e.IdRol);
                entity.Property(e => e.IdRol).HasColumnName("id_rol");
                entity.Property(e => e.RolNombre).HasColumnName("rol_nombre").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(500);
            });

            // Configuración de la tabla Programa
            modelBuilder.Entity<Programa>(entity =>
            {
                entity.ToTable("Programa");
                entity.HasKey(e => e.IdPrograma);
                entity.Property(e => e.IdPrograma).HasColumnName("id_programa");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(500);
            });

            // Configuración de la tabla intermedia Usuario_rol (con id_programa opcional)
            modelBuilder.Entity<UsuarioRol>(entity =>
            {
                entity.ToTable("Usuario_rol");
                
                entity.HasKey(ur => ur.IdUsuarioRol);
                
                entity.Property(ur => ur.IdUsuarioRol).HasColumnName("id_usuario_rol");
                entity.Property(ur => ur.IdUsuario).HasColumnName("id_usuario");
                entity.Property(ur => ur.IdRol).HasColumnName("id_rol");
                entity.Property(ur => ur.IdPrograma).HasColumnName("id_programa"); // Opcional / Nullable

                // Relación con Usuario
                modelBuilder.Entity<UsuarioRol>()
                    .HasOne(ur => ur.Usuario)
                    .WithMany(u => u.UsuarioRoles)
                    .HasForeignKey(ur => ur.IdUsuario);

                // Relación con Rol
                modelBuilder.Entity<UsuarioRol>()
                    .HasOne(ur => ur.Rol)
                    .WithMany(r => r.UsuarioRoles)
                    .HasForeignKey(ur => ur.IdRol);

                // Relación con Programa (Opcional: IsRequired(false))
                modelBuilder.Entity<UsuarioRol>()
                    .HasOne(ur => ur.Programa)
                    .WithMany(p => p.UsuarioRoles)
                    .HasForeignKey(ur => ur.IdPrograma)
                    .IsRequired(false);
            });

            // Configuración de Usuario (Sin contraseña, usando IdentityProviderId)
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");
                entity.HasKey(e => e.IdUsuario);
                
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entity.Property(e => e.IdentityProviderId).HasColumnName("identity_provider_id").IsRequired().HasMaxLength(255);
                // Dentro de modelBuilder.Entity<Usuario>(entity => { ... })
                entity.Property(e => e.Estado).HasColumnName("estado").IsRequired().HasMaxLength(20).HasDefaultValue("Activo");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(50);
                entity.Property(e => e.SegundoNombre).HasColumnName("segundo_nombre").HasMaxLength(50);
                entity.Property(e => e.Apellido).HasColumnName("apellido").IsRequired().HasMaxLength(50);
                entity.Property(e => e.SegundoApellido).HasColumnName("segundo_apellido").HasMaxLength(50);
                entity.Property(e => e.CorreoInstitucional).HasColumnName("correo_institucional").IsRequired().HasMaxLength(100);

                // --- AGREGALAS AQUÍ ---
                entity.HasIndex(e => e.CorreoInstitucional).IsUnique();
                entity.HasIndex(e => e.IdentityProviderId).IsUnique();
            });

            // Configuración de la tabla Estudiante (Perfil general)
            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.ToTable("Estudiante");
                entity.HasKey(e => e.IdEstudiante);

                entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                // Relación 1 a 1 entre Usuario y Estudiante
                entity.HasOne(e => e.Usuario)
                      .WithOne(u => u.Estudiante)
                      .HasForeignKey<Estudiante>(e => e.IdUsuario);
            });

            // Configuración de la tabla Estudiante_programa (Doble titulación y detalles académicos)
            modelBuilder.Entity<EstudiantePrograma>(entity =>
            {
                entity.ToTable("Estudiante_programa");
                entity.HasKey(e => e.IdEstudiantePrograma);

                entity.Property(e => e.IdEstudiantePrograma).HasColumnName("id_estudiante_programa");
                entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
                entity.Property(e => e.IdPrograma).HasColumnName("id_programa");
                entity.Property(e => e.Semestre).HasColumnName("semestre").IsRequired();
                entity.Property(e => e.CodigoEstudiantil).HasColumnName("codigo_estudiantil").IsRequired().HasMaxLength(100);
                entity.Property(e => e.SituacionAcademica).HasColumnName("situacion_academica").IsRequired().HasMaxLength(100);
                entity.Property(e => e.PorcentajeCreditosAprobados).HasColumnName("porcentaje_creditos_aprobados").HasColumnType("decimal(5,2)").IsRequired();
                entity.Property(e => e.PlanEstudio).HasColumnName("plan_estudio").IsRequired().HasMaxLength(100);

                // Relación con Estudiante
                entity.HasOne(ep => ep.Estudiante)
                      .WithMany(e => e.EstudianteProgramas)
                      .HasForeignKey(ep => ep.IdEstudiante);

                // Relación con Programa
                entity.HasOne(ep => ep.Programa)
                      .WithMany(p => p.EstudianteProgramas)
                      .HasForeignKey(ep => ep.IdPrograma);
            });

            // Datos semilla (Seed Data)
            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, RolNombre = "Estudiante", Descripcion = "Rol Estudiante" },
                new Rol { IdRol = 2, RolNombre = "Coordinador", Descripcion = "Rol Coordinador" },
                new Rol { IdRol = 3, RolNombre = "Administrador", Descripcion = "Rol Administrador del Sistema" } // <--- Añadido
            );

            modelBuilder.Entity<Programa>().HasData(
                new Programa { IdPrograma = 1, Nombre = "Ingeniería de Sistemas", Descripcion = "Ingeniería" }
            );
        }
    }
}