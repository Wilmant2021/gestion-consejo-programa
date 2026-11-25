using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.Requests.Domain
{
    [Table("TipoSolicitud")]
    public class TipoSolicitud
    {
        [Key]
        [Column("id_tipo_solicitud")]
        public int IdTipoSolicitud { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(250)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        // Relación con las solicitudes (Tabla Solicitud)
        public ICollection<SubTipoSolicitud> SubTipoSolicitud { get; set; } = new List<SubTipoSolicitud>();
    }
}