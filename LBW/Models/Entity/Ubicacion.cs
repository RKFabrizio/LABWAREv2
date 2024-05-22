namespace LBW.Models.Entity
{
    public class Ubicacion
    {
        public Ubicacion() 
        {
            MuestraU = new HashSet<Muestra>();
        }
        public int ID_LOCATION { get; set; }
        public string? Name_location { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }

        public virtual ICollection<Muestra> MuestraU { get; set; }

    }
}
