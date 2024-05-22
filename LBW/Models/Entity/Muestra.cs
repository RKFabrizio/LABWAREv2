﻿using System.ComponentModel.DataAnnotations;

namespace LBW.Models.Entity
{
    public partial class Muestra
    {
        public Muestra() 
        {
            ResultadosM = new HashSet<Resultado>();
        }
        public int IdSample { get; set; }
        public int IdPm { get; set; }
        public int IdCliente { get; set; }
        public int? IdLocation { get; set; }

        [Required(ErrorMessage = "El número de muestra es requerido.")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "El número de muestra debe ser un número mayor a 0.")]
        public string? SampleNumber { get; set; }
        public string? TextID { get; set; }
        public int Status { get; set; }
        public DateTime? ChangedOn { get; set; }
        public int? OriginalSample { get; set; }
        public DateTime? LoginDate { get; set; }
        public int LoginBy { get; set; }
        public DateTime? SampleDate { get; set; }
        public DateTime? RecdDate { get; set; }
        public int ReceivedBy { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DateCompleted { set; get; }
        public DateTime? DateReviewed { get; set; }
        public string? PreBy { get; set; }
        public string? Reviewer { get; set; }
        public string? SamplingPoint { get; set; }
        public int? SampleType { get; set; }
        public int? IdProject { get; set; }
        public string? SampleName { get; set; }
        public string? Location { get; set; }
        public string? Customer { get; set; }
        public string? Observaciones { get; set; }
        public int? IdPlanta { get; set; }
        public string? ConteoPuntos { get; set; }



        public virtual Lista IdStatusNavigation { get; set; }
        public virtual Usuario IdReceivedByNavigation { get; set; }
        public virtual Usuario IdLoginByNavigation { get; set; }
        public virtual Ubicacion IdUbicacionNavigation { get; set; }
        public virtual PuntoMuestra IdPuntoMuestraNavigation { get; set; }
        public virtual Proyecto IdProyectoNavigation { get; set; }
        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Planta IdPlantaNavigation { get; set; }
        public virtual Lista IdListaNavigation { get; set; }
        public virtual ICollection<Resultado> ResultadosM { get; set; }
    }
}
