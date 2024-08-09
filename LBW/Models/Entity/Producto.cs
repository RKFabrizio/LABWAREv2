using System.ComponentModel.DataAnnotations;

namespace LBW.Models.Entity
{
    public partial class Producto
    {
        public Producto()
        {
            Muestras = new HashSet<Muestra>();
        }
        public int? IdProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }


        public virtual ICollection<Muestra> Muestras { get; set; }
    }
}
