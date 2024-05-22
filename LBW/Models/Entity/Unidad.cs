namespace LBW.Models.Entity
{
    public partial class Unidad
    {
        public Unidad() 
        {
            AnalisisDetallesU = new HashSet<AnalisisDetalle>();
            ResultadosU = new HashSet<Resultado>();
        }
        public int IdUnidad { get; set; }
        public string? NameUnidad { get; set; }
        public string? DisplayString { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
        public bool? Removed { get; set; }
        public string? Description { get; set; }

        public virtual Usuario IdChangedByNavigationIdUser { get; set; }
        public virtual ICollection<AnalisisDetalle> AnalisisDetallesU { get; set; }
        public virtual ICollection<Resultado> ResultadosU { get; set; }
    }
}
