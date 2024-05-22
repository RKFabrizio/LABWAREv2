namespace LBW.Models.Entity
{
    public class Proyecto
    {
        public Proyecto() 
        {
            MuestraPr = new HashSet<Muestra>();
        }

        public int IdProyecto { get; set; }
        public int ID_TL { get; set; }
        public int ID_Cliente { get; set; }
        public string? Name { get; set; }
        public int? TemplateVersion { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public int? Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateRecieved { get; set; }
        public DateTime? DateStarted { get; set;}
        public DateTime? DateCompleted { get; set;}
        public DateTime? DateReviewed { get; set;}
        public DateTime? DateUpdated { get; set;}
        public int? Owner { get; set; }
        public int? NumSamples { get; set; }

        public virtual Lista IdStatusNavigation { get; set; }
        public virtual Usuario IdOwnerNavigation { get; set; }
        public virtual Plantilla IdPlantillaNavigationPr { get; set; }
        public virtual Cliente IdClienteNavigationPr { get; set; }
        public virtual ICollection<Muestra> MuestraPr { get; set; }
    }
}
