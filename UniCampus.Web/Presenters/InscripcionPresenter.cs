using System;
using System.Collections.Generic;
using System.Linq;
using UniCampus.BLL;
using UniCampus.Entities;
using UniCampus.Web.Views;

namespace UniCampus.Web.Presenters
{
    public class InscripcionPresenter
    {
        private IInscripcionView vista;
        private InscripcionService servicio;

        public InscripcionPresenter(IInscripcionView vista)
        {
            this.vista = vista;
            servicio = new InscripcionService();
        }

        public void CargarInscripciones(int usuarioActualId, bool esDocente)
        {
            try
            {
                if (usuarioActualId <= 0)
                    throw new Exception("La sesión no es válida.");

                List<Inscripcion> inscripciones = servicio.ObtenerInscripciones();
                if (!esDocente)
                    inscripciones = inscripciones.Where(i => i.UsuarioId == usuarioActualId).ToList();

                vista.MostrarInscripciones(inscripciones);
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Error al consultar inscripciones: " + ex.Message);
            }
        }

        public bool GuardarInscripcion(Inscripcion inscripcion, int usuarioActualId, bool esDocente)
        {
            try
            {
                if (usuarioActualId <= 0)
                    throw new Exception("La sesión no es válida.");
                if (inscripcion == null)
                    throw new Exception("La inscripción no es válida.");
                if (!esDocente && inscripcion.UsuarioId != usuarioActualId)
                    throw new Exception("Solo puede inscribirse a sí mismo.");

                servicio.GuardarInscripcion(inscripcion);
                vista.MostrarMensaje("Inscripción registrada correctamente.");
                CargarInscripciones(usuarioActualId, esDocente);
                return true;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Error al registrar inscripción: " + ex.Message);
                return false;
            }
        }

        public void CancelarInscripcion(int inscripcionId, int usuarioActualId, bool esDocente)
        {
            try
            {
                if (usuarioActualId <= 0)
                    throw new Exception("La sesión no es válida.");
                if (inscripcionId <= 0)
                    throw new Exception("La inscripción no es válida.");

                // Validar en el servidor: nunca confiar en el botón ni en su CommandArgument.
                Inscripcion inscripcion = servicio.ObtenerInscripciones()
                    .FirstOrDefault(i => i.InscripcionId == inscripcionId);

                if (inscripcion == null)
                    throw new Exception("La inscripción no existe.");
                if (!esDocente && inscripcion.UsuarioId != usuarioActualId)
                    throw new Exception("No tiene permisos para cancelar esta inscripción.");
                if (inscripcion.Estado != "Activa")
                    throw new Exception("La inscripción ya no está activa.");

                servicio.CancelarInscripcion(inscripcionId);
                vista.MostrarMensaje("Inscripción cancelada correctamente.");
                CargarInscripciones(usuarioActualId, esDocente);
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Error al cancelar inscripción: " + ex.Message);
            }
        }
    }
}
