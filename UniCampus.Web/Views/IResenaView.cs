
using System.Collections.Generic;
using UniCampus.Entities;

namespace UniCampus.Web.Views
{
    public interface IResenaView
    {
        // MOSTRAR RESENAS REGISTRADAS
        void MostrarResenas(List<Resena> resenas);

        // MOSTRAR MENSAJES AL USUARIO
        void MostrarMensaje(string mensaje);
    }
}
