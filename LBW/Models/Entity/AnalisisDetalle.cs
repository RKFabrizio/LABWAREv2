namespace LBW.Models.Entity
{
    public partial class AnalisisDetalle
    {
       
        public int IdComp { get; set; }
        public int IdAnalisis { get; set; }
        public int IdUnidad { get; set; }
        public string? NameComponent { get; set; }
        public int? Version { get; set; }
        public string? AnalisisData { get; set; }
        public string? Units { get; set; }
        public int? Minimun {  get; set; }
        public int? Maximun { get; set; }
        public bool? Reportable { get; set; }
        public string? ClampLow { get; set; }
        public string? ClampHigh { get; set; }

        public virtual Analisis IdAnalisisNavigation { get; set; }
        public virtual Unidad IdUnidadNavitation { get; set; }
    }
}
