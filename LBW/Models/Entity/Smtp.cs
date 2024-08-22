namespace LBW.Models.Entity
{
    public partial class Smtp
    { 
        public int ID { get; set; }
        public string Correo { get; set; }
        public string Host { get; set; }
        public int Puerto { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; } 
        public string Body { get; set; }
        public string Ip { get; set; }
    }
}
