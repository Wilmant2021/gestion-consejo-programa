using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.Profile.Domain
{
    [Table("Sub_tipo_solicitud")]
    public class SubTipoSolicitud
    {
        [Key]
        [Column("id_sub_tipo_solicitud")]
        public int IdSubTipoSolicitud { get; set; }

        [Required]
        [Column("id_tipo_soliciud")]
        public int IdTipoSolicitud { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        // Propiedad de navegación hacia el TipoSolicitud padre
        [ForeignKey(nameof(IdTipoSolicitud))]
        public TipoSolicitud? TipoSolicitud { get; set; }
    }
}