using LBW.Controllers;

namespace LBW.Models.Entity
{
    public partial class Grado
    {
        public Grado()
        {
            Muestras = new HashSet<Muestra>();
        }

        public int IdGrado { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Muestra> Muestras { get; set; }
    }
}
