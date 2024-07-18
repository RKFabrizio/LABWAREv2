using System.ComponentModel.DataAnnotations;


namespace LBW.Models.Entity
{
    public partial class AuditoriaResultado
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime Fecha_N { get; set; }
        public string? Analisis { get; set; }
        public string? Muestra { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public int? Login { get; set; }
        public string? Cliente { get; set; }
        public string? Proyecto { get; set; }

    }
}
