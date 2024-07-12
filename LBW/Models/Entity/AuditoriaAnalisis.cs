using System.ComponentModel.DataAnnotations;

namespace LBW.Models.Entity
{
    public partial class AuditoriaAnalisis
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "No te olvides de seleccionar tu análisis.")]
        public int IdAnalisis { get; set; }

        [Required(ErrorMessage = "El Tipo de Analisis es obligatorio")]
        public int IdTipoA { get; set; }
        public string? NameAnalisis { get; set; }
        public int? Version { get; set; }
        public bool? Active { get; set; }
        public string? CommonName { get; set; }
        public string? Description { get; set; }
        public string? AliasName { get; set; }
        public DateTime? ChangedOn { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? Action { get; set; }
        public int? Login { get; set; }
    }
}
