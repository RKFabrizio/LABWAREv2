namespace LBW.Models.Entity
{
    public partial class Site
    {
        public Site() 
        {
            PlantasS = new HashSet<Planta>();
            ClienteS = new HashSet<Cliente>();
        }
        public int IdSite {  get; set; }
        public string? NameSite { get; set; }
        public string? Compania { get; set; }


        public virtual ICollection<Planta> PlantasS { get; set; }
        public virtual ICollection<Cliente> ClienteS { get; set; }
    }
}
