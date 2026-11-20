
using System.Collections.Generic;
using System.Data;
using UniCampus.Entities;

namespace UniCampus.Web.Views
{
    public interface IActividadView
    {
        // MOSTRAR ACTIVIDADES
        void MostrarActividades(List<ActividadAcademica> actividades);

        // MOSTRAR TIPOS DE ACTIVIDAD
        void MostrarTiposActividad(DataTable tipos);

        // MOSTRAR MENSAJES
        void MostrarMensaje(string mensaje);
    }
}
