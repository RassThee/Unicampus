
using System;
using System.Data;
using System.Net.Mail;
using UniCampus.DAL;

namespace UniCampus.BLL
{
    public class UsuarioService
    {
        private UsuarioRepository repositorio =
            new UsuarioRepository();

        // OBTENER TODOS LOS ESTUDIANTES
        public DataTable ObtenerEstudiantes()
        {
            return repositorio.ObtenerEstudiantes();
        }

        // OBTENER ESTUDIANTES ACTIVOS
        public DataTable ObtenerEstudiantesActivos()
        {
            return repositorio.ObtenerEstudiantesActivos();
        }

        // BUSCAR ESTUDIANTE POR ID
        public DataRow ObtenerEstudiantePorId(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new Exception(
                    "El ID del estudiante no es válido.");
            }

            DataRow estudiante =
                repositorio.ObtenerEstudiantePorId(usuarioId);

            if (estudiante == null)
            {
                throw new Exception(
                    "No se encontró el estudiante.");
            }

            return estudiante;
        }

        // VALIDAR DATOS DEL ESTUDIANTE
        private void ValidarDatos(
            string nombre,
            string correo,
            string programa)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new Exception(
                    "Debe ingresar el nombre del estudiante.");
            }

            if (string.IsNullOrWhiteSpace(correo))
            {
                throw new Exception(
                    "Debe ingresar el correo electrónico.");
            }

            try
            {
                MailAddress direccion =
                    new MailAddress(correo);

                if (direccion.Address != correo)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new Exception(
                    "El correo electrónico no tiene un formato válido.");
            }

            if (string.IsNullOrWhiteSpace(programa))
            {
                throw new Exception(
                    "Debe ingresar el programa académico.");
            }
        }

        // VALIDAR CONTRASEÑA
        private void ValidarPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception(
                    "Debe ingresar una contraseña.");
            }

            if (password.Length < 8)
            {
                throw new Exception(
                    "La contraseña debe tener al menos 8 caracteres.");
            }

            if (password.Length > 128)
            {
                throw new Exception(
                    "La contraseña no puede superar los 128 caracteres.");
            }

            if (password != password.Trim())
            {
                throw new Exception(
                    "La contraseña no debe comenzar ni terminar con espacios.");
            }
        }

        // REGISTRAR ESTUDIANTE CON CONTRASEÑA
        public void RegistrarEstudiante(
            string nombre,
            string correo,
            string programa,
            string password)
        {
            nombre = (nombre ?? "").Trim();
            correo = (correo ?? "").Trim();
            programa = (programa ?? "").Trim();

            ValidarDatos(nombre, correo, programa);
            ValidarPassword(password);

            if (repositorio.ExisteCorreo(correo))
            {
                throw new Exception(
                    "Ya existe un usuario registrado con ese correo.");
            }

            // GENERAR HASH SEGURO
            string passwordHash =
                SeguridadPassword.GenerarHash(password);

            // GUARDAR ESTUDIANTE CON SU CONTRASEÑA
            repositorio.RegistrarEstudiante(
                nombre,
                correo,
                programa,
                passwordHash);
        }

        // COMPATIBILIDAD CON CODIGO ANTERIOR
        // IMPIDE CREAR CUENTAS SIN CONTRASEÑA
        public void RegistrarEstudiante(
            string nombre,
            string correo,
            string programa)
        {
            throw new InvalidOperationException(
                "Debe asignar una contraseña inicial al estudiante.");
        }

        // MODIFICAR ESTUDIANTE
        public void ModificarEstudiante(
            int usuarioId,
            string nombre,
            string correo,
            string programa)
        {
            if (usuarioId <= 0)
            {
                throw new Exception(
                    "El ID del estudiante no es válido.");
            }

            nombre = (nombre ?? "").Trim();
            correo = (correo ?? "").Trim();
            programa = (programa ?? "").Trim();

            ValidarDatos(nombre, correo, programa);

            ObtenerEstudiantePorId(usuarioId);

            if (repositorio.ExisteCorreoEnOtroUsuario(
                correo, usuarioId))
            {
                throw new Exception(
                    "El correo electrónico ya pertenece a otro usuario.");
            }

            repositorio.ModificarEstudiante(
                usuarioId,
                nombre,
                correo,
                programa);
        }

        // RESTABLECER CONTRASEÑA
        public void RestablecerPassword(
            int usuarioId,
            string nuevaPassword)
        {
            if (usuarioId <= 0)
            {
                throw new Exception(
                    "El ID del estudiante no es válido.");
            }

            ValidarPassword(nuevaPassword);

            // COMPROBAR QUE EL ESTUDIANTE EXISTA
            ObtenerEstudiantePorId(usuarioId);

            // GENERAR NUEVO HASH
            string nuevoHash =
                SeguridadPassword.GenerarHash(nuevaPassword);

            // ACTUALIZAR CONTRASEÑA EN SQL SERVER
            repositorio.RestablecerPassword(
                usuarioId,
                nuevoHash);
        }

        // DESACTIVAR ESTUDIANTE
        public void DesactivarEstudiante(int usuarioId)
        {
            DataRow estudiante =
                ObtenerEstudiantePorId(usuarioId);

            if (estudiante["Estado"].ToString() == "Inactivo")
            {
                throw new Exception(
                    "El estudiante ya se encuentra inactivo.");
            }

            repositorio.CambiarEstadoEstudiante(
                usuarioId,
                "Inactivo");
        }

        // REACTIVAR ESTUDIANTE
        public void ReactivarEstudiante(int usuarioId)
        {
            DataRow estudiante =
                ObtenerEstudiantePorId(usuarioId);

            if (estudiante["Estado"].ToString() == "Activo")
            {
                throw new Exception(
                    "El estudiante ya se encuentra activo.");
            }

            repositorio.CambiarEstadoEstudiante(
                usuarioId,
                "Activo");
        }
    }
}
