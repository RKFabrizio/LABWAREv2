﻿using System.ComponentModel.DataAnnotations;


namespace LBW.Models.Entity
{
    public partial class AuditoriaResultado
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Resultado")]
        public int IdResult { get; set; }

        [Required]
        [Display(Name = "Muestra")]
        public int IdSample { get; set; }

        [Required]
        [Display(Name = "Unidad")]
        public int IdUnidad { get; set; }

        [Required]
        [Display(Name = "Analisis Detalle")]
        public int IdComponent { get; set; }

        [Required]
        [Display(Name = "Análisis")]
        public int IdAnalysis { get; set; }

        public string? SampleNumber { get; set; }


        public decimal? ResultNumber { get; set; }
        public int? OrderNum { get; set; }
        public string? AnalysisData { get; set; }
        public string? NameComponent { get; set; }
        public string? ReportedName { get; set; }
        public int? Status { get; set; }
        public bool? Reportable { get; set; }
        public DateTime? ChangedOn { get; set; }
        public int? Instrument { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? Action { get; set; }
        public int? Login { get; set; }
    }
}