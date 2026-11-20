
using System;

namespace UniCampus.Entities
{
    public class Inscripcion
    {
        public int InscripcionId { get; set; }

        public int ActividadId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime FechaInscripcion { get; set; }

        public string Estado { get; set; }

        // Datos adicionales para mostrar en la tabla
        public string NombreActividad { get; set; }

        public string NombreEstudiante { get; set; }

        public string Programa { get; set; }
    }
}
