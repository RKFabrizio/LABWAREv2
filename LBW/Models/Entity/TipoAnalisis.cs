using System.ComponentModel.DataAnnotations;

namespace LBW.Models.Entity
{
    public partial class TipoAnalisis
    {
        public TipoAnalisis() 
        {
            Analisises = new HashSet<Analisis>();    
        }

        public int IdTipoA { get; set; }
        public string? NombreA { get; set; }
        public string? Descripcion { get; set; }
        public bool? Removed { get; set; }

        public virtual ICollection<Analisis> Analisises { get; set;}
    }
}
