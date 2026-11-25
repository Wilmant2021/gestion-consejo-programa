using Microsoft.EntityFrameworkCore;
using Modules.Requests.Domain;

namespace Plataforma.Modules.Requests.Infrastructure
{
    public class RequestsDbContext : DbContext
    {
        public RequestsDbContext(DbContextOptions<RequestsDbContext> options) : base(options)
        {
        }

        public DbSet<TipoSolicitud> TipoSolicitudes { get; set; }
        public DbSet<SubTipoSolicitud> SubTipoSolicitudes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Esquema propio del módulo de solicitudes para aislarlo de "perfil"
            modelBuilder.HasDefaultSchema("requests");

            // Configuración de la tabla Tipo_solicitud
            modelBuilder.Entity<TipoSolicitud>(entity =>
            {
                entity.ToTable("Tipo_solicitud");
                entity.HasKey(e => e.IdTipoSolicitud);

                entity.Property(e => e.IdTipoSolicitud)
                    .HasColumnName("id_tipo_solicitud");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                    .HasColumnName("drescripcion")
                    .HasMaxLength(500);

                // Relación uno a muchos: un TipoSolicitud tiene varios SubTipos
                entity.HasMany(e => e.SubTipoSolicitud)
                    .WithOne(s => s.TipoSolicitud)
                    .HasForeignKey(s => s.IdTipoSolicitud)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de la tabla Sub_tipo_solicitud
            modelBuilder.Entity<SubTipoSolicitud>(entity =>
            {
                entity.ToTable("Sub_tipo_solicitud");
                entity.HasKey(e => e.IdSubTipoSolicitud);

                entity.Property(e => e.IdSubTipoSolicitud)
                    .HasColumnName("id_sub_tipo_solicitud");

                entity.Property(e => e.IdTipoSolicitud)
                    .HasColumnName("id_tipo_soliciud")
                    .IsRequired();

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(500);
            });
        }
    }
}