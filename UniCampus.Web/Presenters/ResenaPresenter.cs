
using System;
using UniCampus.BLL;
using UniCampus.Entities;
using UniCampus.Web.Views;

namespace UniCampus.Web.Presenters
{
    public class ResenaPresenter
    {
        private IResenaView vista;
        private ResenaService servicio;

        public ResenaPresenter(IResenaView vista)
        {
            this.vista = vista;
            servicio = new ResenaService();
        }

        // CONSULTAR RESENAS
        public void CargarResenas()
        {
            try
            {
                vista.MostrarResenas(
                    servicio.ObtenerResenas());
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje(
                    "Error al consultar reseñas: "
                    + ex.Message);
            }
        }

        // GUARDAR RESENA
        public bool GuardarResena(Resena resena)
        {
            try
            {
                servicio.GuardarResena(resena);

                CargarResenas();

                vista.MostrarMensaje(
                    "Reseña registrada correctamente.");

                return true;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje(
                    "Error al registrar reseña: "
                    + ex.Message);

                return false;
            }
        }

        // MODIFICAR RESENA
        public bool ModificarResena(Resena resena)
        {
            try
            {
                // ACTUALIZAR RESEÑA EN SQL SERVER
                servicio.ModificarResena(resena);

                // ACTUALIZAR TABLA
                CargarResenas();

                // MOSTRAR CONFIRMACION
                vista.MostrarMensaje(
                    "Reseña modificada correctamente.");

                return true;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje(
                    "Error al modificar reseña: "
                    + ex.Message);

                return false;
            }
        }

        // ELIMINAR RESENA
        public bool EliminarResena(int resenaId)
        {
            try
            {
                // ELIMINAR RESEÑA EN SQL SERVER
                servicio.EliminarResena(resenaId);

                // ACTUALIZAR TABLA
                CargarResenas();

                // MOSTRAR CONFIRMACION
                vista.MostrarMensaje(
                    "Reseña eliminada correctamente.");

                return true;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje(
                    "Error al eliminar reseña: "
                    + ex.Message);

                return false;
            }
        }
    }
}
