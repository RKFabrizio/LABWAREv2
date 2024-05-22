using LBW.Controllers;

namespace LBW.Models.Entity
{
    public partial class Usuario
    {
        public Usuario()
        {
            Analisises = new HashSet<Analisis>();
            Clientes = new HashSet<Cliente>();
            Muestras1 = new HashSet<Muestra>();
            Muestras2 = new HashSet<Muestra>();
            Plantas = new HashSet<Planta>();
            Proyectos = new HashSet<Proyecto>();
            PuntosMuestra = new HashSet<PuntoMuestra>();
            Unidades = new HashSet<Unidad>();
        }

        public int IdUser { get; set; }
        public string? UsuarioID  { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }
        public bool? Rol { get; set; }
        public int? GMT_OFFSET { get; set; }
        public bool? UsuarioDeshabilitado { get; set; }
        public DateTime? FechaDeshabilitado { get; set; }
        public int? CCliente { get; set; }

        public virtual Cliente IdCClienteNavigation { get; set; }
        public virtual ICollection<Unidad> Unidades { get; set; }
        public virtual ICollection<Analisis> Analisises { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<Muestra> Muestras1 { get; set; }
        public virtual ICollection<Muestra> Muestras2 { get; set; }
        public virtual ICollection<Planta> Plantas { get; set; }
        public virtual ICollection<Proyecto> Proyectos { get; set; }
        public virtual ICollection<PuntoMuestra> PuntosMuestra { get; set; }
    }
}
