namespace LBW.Models.Entity
{
    public partial class Lista
    {
        public Lista()
        {
            Muestras = new HashSet<Muestra>();
            Muestras1 = new HashSet<Muestra>();
            Proyectos = new HashSet<Proyecto>();
            Resultados = new HashSet<Resultado>();
        }
        public int IdLista { get; set; }
        public string? List {  get; set; }
        public string? NameLista { get; set; }
        public string? Value { get; set; }
        public int? OrderNumber { get; set; }

        public virtual ICollection<Muestra> Muestras { get; set; }
        public virtual ICollection<Muestra> Muestras1 { get; set; }
        public virtual ICollection<Proyecto> Proyectos { get; set; }
        public virtual ICollection<Resultado> Resultados { get; set; }
    }
}
