namespace LBW.Models.Entity
{
    public partial class PuntoMuestra
    {
        public PuntoMuestra()
        {
            MuestraPm = new HashSet<Muestra>();
        }
        public int IdPm { get; set; }
        public int IdPlanta { get; set; }
        public string? NamePm { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
        public string? Description { get; set; }
        public string? C_CodPunto {  get; set; }

        public virtual Usuario IdChangedByNavigation { get; set; }
        public virtual Planta IdPlantaNavigation { get; set; }
        public virtual ICollection<Muestra> MuestraPm { get; set; }
    }
}
