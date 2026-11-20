
using System;
using System.Collections.Generic;
using System.Data;
using UniCampus.DAL;
using UniCampus.Entities;

namespace UniCampus.BLL
{
    public class InscripcionService
    {
        private InscripcionRepository repositorio;

        public InscripcionService()
        {
            repositorio = new InscripcionRepository();
        }

        // CONSULTAR INSCRIPCIONES
        public List<Inscripcion> ObtenerInscripciones()
        {
            return repositorio.ObtenerInscripciones();
        }

        // CONSULTAR ESTUDIANTES INSCRITOS EN UNA ACTIVIDAD
        public DataTable ObtenerEstudiantesInscritos(int actividadId)
        {
            // VALIDAR ACTIVIDAD
            if (actividadId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar una actividad válida.");
            }

            // CONSULTAR ESTUDIANTES EN LA CAPA DAL
            DataTable estudiantes =
                repositorio.ObtenerEstudiantesInscritos(actividadId);

            return estudiantes;
        }

        // VALIDAR Y GUARDAR INSCRIPCION
        public void GuardarInscripcion(Inscripcion inscripcion)
        {
            if (inscripcion == null)
            {
                throw new Exception(
                    "Debe ingresar los datos de la inscripción.");
            }

            if (inscripcion.ActividadId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una actividad válida.");
            }

            if (inscripcion.UsuarioId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar un usuario válido.");
            }

            repositorio.GuardarInscripcion(inscripcion);
        }

        // VALIDAR Y CANCELAR INSCRIPCION
        public void CancelarInscripcion(int inscripcionId)
        {
            if (inscripcionId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una inscripción válida.");
            }

            repositorio.CancelarInscripcion(inscripcionId);
        }
    }
}
