
using System;
using System.Collections.Generic;
using System.Data;
using UniCampus.DAL;
using UniCampus.Entities;

namespace UniCampus.BLL
{
    public class ActividadService
    {
        private ActividadRepository repositorio =
            new ActividadRepository();

        // CONSULTAR ACTIVIDADES
        public List<ActividadAcademica> ObtenerActividades()
        {
            return repositorio.ObtenerActividades();
        }

        // BUSCAR ACTIVIDADES POR NOMBRE
        public List<ActividadAcademica> BuscarActividades(string nombre)
        {
            // SI NO SE INGRESA TEXTO, MOSTRAR TODAS
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return repositorio.ObtenerActividades();
            }

            // ELIMINAR ESPACIOS AL INICIO Y AL FINAL
            nombre = nombre.Trim();

            // BUSCAR EN LA CAPA DAL
            return repositorio.BuscarActividades(nombre);
        }

        // FILTRAR ACTIVIDADES ACADEMICAS
        public List<ActividadAcademica> FiltrarActividades(
            string nombre, DateTime? fechaInicio,
            int tipoId, string estado)
        {
            // VALIDAR TIPO DE ACTIVIDAD
            if (tipoId < 0)
            {
                throw new ArgumentException(
                    "El tipo de actividad no es valido.");
            }

            // VALIDAR ESTADO
            if (!string.IsNullOrWhiteSpace(estado))
            {
                estado = estado.Trim();

                if (estado != "Borrador" &&
                    estado != "Publicada" &&
                    estado != "Cancelada" &&
                    estado != "Finalizada")
                {
                    throw new ArgumentException(
                        "El estado seleccionado no es valido.");
                }
            }

            // LIMPIAR TEXTO DE BUSQUEDA
            if (nombre == null)
            {
                nombre = "";
            }

            nombre = nombre.Trim();

            // CONSULTAR LOS FILTROS EN LA CAPA DAL
            return repositorio.FiltrarActividades(
                nombre, fechaInicio, tipoId, estado);
        }

        // CONSULTAR TIPOS DE ACTIVIDAD
        public DataTable ObtenerTiposActividad()
        {
            return repositorio.ObtenerTiposActividad();
        }

        // CONSULTAR CUPOS DE UNA ACTIVIDAD
        public DataTable ObtenerCuposActividad(int actividadId)
        {
            // VALIDAR EL ID DE LA ACTIVIDAD
            if (actividadId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar una actividad valida.");
            }

            // CONSULTAR CUPOS EN LA CAPA DAL
            DataTable tabla =
                repositorio.ObtenerCuposActividad(actividadId);

            // VERIFICAR QUE LA ACTIVIDAD EXISTA
            if (tabla.Rows.Count == 0)
            {
                throw new ArgumentException(
                    "No se encontro la actividad seleccionada.");
            }

            return tabla;
        }

        // VALIDAR DATOS DE LA ACTIVIDAD
        private void ValidarActividad(ActividadAcademica actividad)
        {
            if (actividad == null)
            {
                throw new ArgumentException(
                    "Debe ingresar los datos de la actividad.");
            }

            // VALIDAR TITULO
            if (string.IsNullOrWhiteSpace(actividad.Titulo))
            {
                throw new ArgumentException(
                    "Debe ingresar el titulo de la actividad.");
            }

            // VALIDAR TIPO
            if (actividad.TipoId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar un tipo de actividad.");
            }

            // VALIDAR FECHAS
            if (actividad.FechaFin < actividad.FechaInicio)
            {
                throw new ArgumentException(
                    "La fecha final no puede ser menor que la inicial.");
            }

            // VALIDAR HORARIOS
            if (actividad.HoraFin <= actividad.HoraInicio)
            {
                throw new ArgumentException(
                    "La hora final debe ser mayor que la inicial.");
            }

            // VALIDAR CUPOS
            if (actividad.Cupos <= 0)
            {
                throw new ArgumentException(
                    "Los cupos deben ser mayores que cero.");
            }

            // VALIDAR MODALIDAD
            if (actividad.Modalidad != "Presencial" &&
                actividad.Modalidad != "Virtual" &&
                actividad.Modalidad != "Hibrida")
            {
                throw new ArgumentException(
                    "Debe seleccionar una modalidad valida.");
            }

            // VALIDAR ESTADO
            if (actividad.Estado != "Borrador" &&
                actividad.Estado != "Publicada" &&
                actividad.Estado != "Cancelada" &&
                actividad.Estado != "Finalizada")
            {
                throw new ArgumentException(
                    "Debe seleccionar un estado valido.");
            }
        }

        // GUARDAR ACTIVIDAD NUEVA
        public void GuardarActividad(ActividadAcademica actividad)
        {
            ValidarActividad(actividad);

            repositorio.GuardarActividad(actividad);
        }

        // ACTUALIZAR ACTIVIDAD EXISTENTE
        public bool ActualizarActividad(ActividadAcademica actividad)
        {
            if (actividad == null)
            {
                throw new ArgumentException(
                    "Debe ingresar los datos de la actividad.");
            }

            if (actividad.ActividadId <= 0)
            {
                throw new ArgumentException(
                    "El ID de la actividad no es valido.");
            }

            ValidarActividad(actividad);

            bool actualizado =
                repositorio.ActualizarActividad(actividad);

            if (!actualizado)
            {
                throw new ArgumentException(
                    "No se encontro la actividad que desea modificar.");
            }

            return true;
        }

        // ELIMINAR ACTIVIDAD EXISTENTE
        public bool EliminarActividad(int actividadId)
        {
            // VALIDAR IDENTIFICADOR
            if (actividadId <= 0)
            {
                throw new ArgumentException(
                    "El ID de la actividad no es valido.");
            }

            // ELIMINAR DESDE LA CAPA DAL
            bool eliminado =
                repositorio.EliminarActividad(actividadId);

            if (!eliminado)
            {
                throw new ArgumentException(
                    "No se encontro la actividad que desea eliminar.");
            }

            return true;
        }
    }
}
