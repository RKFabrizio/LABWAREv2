namespace LBW.Models.Entity
{
    public partial class Instrumento
    {
        public Instrumento()
        {

            Resultados = new HashSet<Resultado>();
        }
        public int IdInstrumento { get; set; }
        public string? IdCodigo { get; set; }
        public string? Descripcion { get; set; }
        public string? Nombre { get; set; }
        public string? Tipo { get; set; }
        public string? Vendor { get; set; }
        public bool? Habilitado { get; set; }
        public DateTime? FechaCalibrado { get; set; }
        public DateTime? FechaCaducidad { get; set; }

        public virtual ICollection<Resultado> Resultados { get; set; }
    }
}
