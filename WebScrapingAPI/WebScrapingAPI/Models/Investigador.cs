﻿namespace WebScrapingAPI.Models
{
    public class Investigador
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public string? Apellidos { get; set; }

        public string? IdInvestigador { get; set; }

        public string? Email { get; set; }

        public int? FoDepartamento { get; set; }
        public Departamento? Departamento { get; set; }

        public int? FoFacultad { get; set; }
        public Facultad? Facultad { get; set; }

        public int? FoArea { get; set; }
        public Area? Area { get; set; }
    }
}
