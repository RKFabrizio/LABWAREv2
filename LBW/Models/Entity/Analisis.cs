using System.ComponentModel.DataAnnotations;
namespace LBW.Models.Entity
{
    public partial class Analisis
    {
        public Analisis() 
        {
            AnalisisDetallesA = new HashSet<AnalisisDetalle>();
            PlantillaDetalleA = new HashSet<PlantillaDetalle>();
            ResultadosA = new HashSet<Resultado>();
        }
        [Required(ErrorMessage = "El Análisis es obligatorio")]
        public int IdAnalisis { get; set; }

        [Required(ErrorMessage = "El Tipo de Analisis es obligatorio")]
        public int IdTipoA { get; set; }
        public string? NameAnalisis { get; set; }
        public int? Version {  get; set; }
        public bool? Active { get; set; }
        public string? CommonName { get; set; }
        public string? Description { get; set; }
        public string? AliasName { get; set; }
        public DateTime? ChangedOn { get; set; }
        public int? ChangedBy { get; set; }

        public virtual TipoAnalisis IdANavigation { get; set;}
        public virtual Usuario IdChangedByNavigation { get; set; }
        public virtual ICollection<AnalisisDetalle> AnalisisDetallesA { get; set; }
        public virtual ICollection<PlantillaDetalle> PlantillaDetalleA { get; set; }
        public virtual ICollection<Resultado> ResultadosA { get; set; }
    }
}
