
using System;

namespace UniCampus.Entities
{
    public class Resena
    {
        // IDENTIFICADOR DE LA RESENA
        public int ResenaId { get; set; }

        // IDENTIFICADOR DE LA ACTIVIDAD
        public int ActividadId { get; set; }

        // NOMBRE DE LA ACTIVIDAD
        public string NombreActividad { get; set; }

        // IDENTIFICADOR DEL ESTUDIANTE
        public int UsuarioId { get; set; }

        // NOMBRE DEL ESTUDIANTE
        public string NombreEstudiante { get; set; }

        // CALIFICACION DE LA ACTIVIDAD
        public byte Calificacion { get; set; }

        // COMENTARIO DEL ESTUDIANTE
        public string Comentario { get; set; }

        // FECHA DE CREACION
        public DateTime FechaCreacion { get; set; }
    }
}
