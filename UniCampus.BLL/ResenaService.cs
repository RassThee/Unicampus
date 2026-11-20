
using System;
using System.Collections.Generic;
using UniCampus.DAL;
using UniCampus.Entities;

namespace UniCampus.BLL
{
    public class ResenaService
    {
        private ResenaRepository repositorio;

        public ResenaService()
        {
            repositorio = new ResenaRepository();
        }

        // CONSULTAR RESENAS
        public List<Resena> ObtenerResenas()
        {
            return repositorio.ObtenerResenas();
        }

        // VALIDAR Y GUARDAR RESENA
        public void GuardarResena(Resena resena)
        {
            if (resena == null)
            {
                throw new Exception(
                    "Debe ingresar los datos de la reseña.");
            }

            // VALIDAR ACTIVIDAD
            if (resena.ActividadId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una actividad válida.");
            }

            // VALIDAR USUARIO
            if (resena.UsuarioId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar un usuario válido.");
            }

            // VALIDAR CALIFICACION
            if (resena.Calificacion < 1 ||
                resena.Calificacion > 5)
            {
                throw new Exception(
                    "La calificación debe estar entre 1 y 5.");
            }

            // ENVIAR LOS DATOS A LA CAPA DAL
            repositorio.GuardarResena(resena);
        }

        // VALIDAR Y MODIFICAR RESENA
        public void ModificarResena(Resena resena)
        {
            // VALIDAR DATOS
            if (resena == null)
            {
                throw new Exception(
                    "Debe ingresar los datos de la reseña.");
            }

            // VALIDAR ID DE LA RESENA
            if (resena.ResenaId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una reseña válida para modificar.");
            }

            // VALIDAR CALIFICACION
            if (resena.Calificacion < 1 ||
                resena.Calificacion > 5)
            {
                throw new Exception(
                    "La calificación debe estar entre 1 y 5.");
            }

            // ENVIAR LOS CAMBIOS A LA CAPA DAL
            repositorio.ModificarResena(resena);
        }

        // VALIDAR Y ELIMINAR RESENA
        public void EliminarResena(int resenaId)
        {
            // VALIDAR ID
            if (resenaId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una reseña válida para eliminar.");
            }

            // ELIMINAR EN LA BASE DE DATOS
            repositorio.EliminarResena(resenaId);
        }
    }
}
