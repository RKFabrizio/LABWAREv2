namespace LBW.Models.Entity
{
    public partial class Cliente
    {
        public Cliente()
        {
            PlantasC = new HashSet<Planta>();
            PlantillaC = new HashSet<Plantilla>();
            ProyectoC = new HashSet<Proyecto>();
            MuestraC = new HashSet<Muestra>();
            Usuarios = new HashSet<Usuario>();
        }
        public int IdCliente { get; set; }
        public int IdSite { get; set; }
        public string? NameCliente { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }
        public DateTime? ChangedOn { get; set; }
        public int? ChangedBy { get; set; }
        public string? EmailAddrs { get; set; }
        public string? C_ClientesAgua {  get; set; }


        public virtual Usuario IdChangedByNavigation { get; set; }
        public virtual Site IdSiteNavigationC { get; set; }
        public virtual ICollection<Planta> PlantasC { get; set; }
        public virtual ICollection<Plantilla> PlantillaC { get; set; }
        public virtual ICollection<Proyecto> ProyectoC { get; set; }
        public virtual ICollection<Muestra> MuestraC { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }

    }
}
