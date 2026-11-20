
using System.Collections.Generic;
using UniCampus.Entities;

namespace UniCampus.Web.Views
{
    public interface IInscripcionView
    {
        // MOSTRAR INSCRIPCIONES
        void MostrarInscripciones(
            List<Inscripcion> inscripciones);

        // MOSTRAR MENSAJES
        void MostrarMensaje(string mensaje);
    }
}
