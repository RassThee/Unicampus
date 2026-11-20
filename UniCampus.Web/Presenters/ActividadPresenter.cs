
using System;
using System.Collections.Generic;
using UniCampus.BLL;
using UniCampus.Entities;
using UniCampus.Web.Views;

namespace UniCampus.Web.Presenters
{
    public class ActividadPresenter
    {
        private IActividadView vista;
        private ActividadService servicio;

        public ActividadPresenter(IActividadView vista)
        {
            this.vista = vista;
            servicio = new ActividadService();
        }

        // CONSULTAR ACTIVIDADES
        public void CargarActividades()
        {
            try
            {
                var actividades = servicio.ObtenerActividades();

                vista.MostrarActividades(actividades);
            }
            catch (Exception)
            {
                vista.MostrarMensaje(
                    "No fue posible cargar las actividades.");
            }
        }

        // BUSCAR ACTIVIDADES POR NOMBRE
        public void BuscarActividades(string nombre)
        {
            try
            {
                var actividades =
                    servicio.BuscarActividades(nombre);

                vista.MostrarActividades(actividades);
            }
            catch (Exception)
            {
                vista.MostrarMensaje(
                    "No fue posible buscar las actividades.");
            }
        }


        // FILTRAR ACTIVIDADES ACADEMICAS
        public void FiltrarActividades(
            string nombre, DateTime? fechaInicio,
            int tipoId, string estado)
        {
            try
            {
                List<ActividadAcademica> actividades =
                    servicio.FiltrarActividades(
                        nombre, fechaInicio, tipoId, estado);

                vista.MostrarActividades(actividades);
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje(
                    "Error al filtrar actividades: " + ex.Message);
            }
        }

        // CONSULTAR TIPOS DE ACTIVIDAD
        public void CargarTiposActividad()
        {
            try
            {
                var tipos = servicio.ObtenerTiposActividad();

                vista.MostrarTiposActividad(tipos);
            }
            catch (Exception)
            {
                vista.MostrarMensaje(
                    "No fue posible cargar los tipos de actividad.");
            }
        }

        // GUARDAR ACTIVIDAD NUEVA
        public bool GuardarActividad(ActividadAcademica actividad)
        {
            try
            {
                servicio.GuardarActividad(actividad);

                vista.MostrarMensaje(
                    "Actividad registrada correctamente.");

                return true;
            }
            catch (ArgumentException ex)
            {
                vista.MostrarMensaje(ex.Message);
                return false;
            }
            catch (Exception)
            {
                vista.MostrarMensaje(
                    "No fue posible registrar la actividad. Revise los datos.");

                return false;
            }
        }

        // ACTUALIZAR ACTIVIDAD EXISTENTE
        public bool ActualizarActividad(ActividadAcademica actividad)
        {
            try
            {
                bool actualizado =
                    servicio.ActualizarActividad(actividad);

                if (actualizado)
                {
                    vista.MostrarMensaje(
                        "Actividad actualizada correctamente.");

                    return true;
                }

                vista.MostrarMensaje(
                    "No se pudo actualizar la actividad.");

                return false;
            }
            catch (ArgumentException ex)
            {
                vista.MostrarMensaje(ex.Message);
                return false;
            }
            catch (Exception)
            {
                vista.MostrarMensaje(
                    "Ocurrio un error al actualizar la actividad.");

                return false;
            }
        }

        // ELIMINAR ACTIVIDAD EXISTENTE
        public bool EliminarActividad(int actividadId)
        {
            try
            {
                bool eliminado =
                    servicio.EliminarActividad(actividadId);

                if (eliminado)
                {
                    vista.MostrarMensaje(
                        "Actividad eliminada correctamente.");

                    return true;
                }

                vista.MostrarMensaje(
                    "No se pudo eliminar la actividad.");

                return false;
            }
            catch (ArgumentException ex)
            {
                vista.MostrarMensaje(ex.Message);
                return false;
            }
            catch (Exception)
            {
                vista.MostrarMensaje(
                    "No fue posible eliminar la actividad. " +
                    "Verifique si tiene inscripciones o resenas relacionadas.");

                return false;
            }
        }
    }
}
