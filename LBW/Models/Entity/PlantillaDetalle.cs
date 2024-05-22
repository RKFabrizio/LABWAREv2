namespace LBW.Models.Entity
{
    public partial class PlantillaDetalle
    {
        public int Id_TLE { get; set; }
        public int Id_TL { get; set; }
        public int Id_Analysis { get; set; }
        public string? Name { get; set; }
        public string? Analysis {  get; set; }
        public int? OrderNumber { get; set; }

        public virtual Analisis IdAnalysisNavigation { get; set; }
        public virtual Plantilla IdTLNavigation { get; set; }
    }
}
