namespace LBW.Models.Entity
{
    public partial class Planta
    {
        public Planta() 
        {
            PuntoMuestrasP = new HashSet<PuntoMuestra>();
            Muestras = new HashSet<Muestra>();
        }

        public int IdPlanta { get; set; }
        public int IdCliente { get; set; }
        public int IdSite { get; set; }
        public string? NamePl { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
        public bool? Removed { get; set; }
        public string? Description { get; set; }

        public virtual Usuario IdChangedByNavigation { get; set; }
        public virtual Site IdSiteNavigationP { get; set; }
        public virtual Cliente IdClienteNavigationP { get; set; }
        public virtual ICollection<PuntoMuestra> PuntoMuestrasP { get; set; }
        public virtual ICollection<Muestra> Muestras { get; set; }
    }

}
