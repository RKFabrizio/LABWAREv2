using System.ComponentModel.DataAnnotations;

namespace LBW.Models
{
    public class Usuario
    {
        public int IdUser { get; set; }
        public string? UsuarioID { get; set; }
        public string Password { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }
        public int? IdRol { get; set; }
        public int? GMT_OFFSET { get; set; }
        public bool? UsuarioDeshabilitado { get; set; }
        public DateTime? FechaDeshabilitado { get; set; }
        public int? CCliente { get; set; }
    }
}
