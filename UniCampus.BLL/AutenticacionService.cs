
using System;
using System.Data;
using UniCampus.DAL;

namespace UniCampus.BLL
{
    public class AutenticacionService
    {
        private AutenticacionRepository repositorio =
            new AutenticacionRepository();

        public DataRow IniciarSesion(
            string correo,
            string password)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrEmpty(password))
            {
                return null;
            }

            // Buscar usuario en SQL Server
            DataTable tabla =
                repositorio.BuscarUsuarioPorCorreo(
                    correo.Trim());

            // El correo debe identificar una sola cuenta
            if (tabla.Rows.Count != 1)
            {
                return null;
            }

            DataRow usuario = tabla.Rows[0];

            // Verificar que el usuario esté activo
            string estado =
                Convert.ToString(usuario["Estado"]);

            if (estado != "Activo")
            {
                return null;
            }

            // Verificar contraseña
            string hashGuardado =
                Convert.ToString(usuario["PasswordHash"]);

            bool passwordCorrecto =
                SeguridadPassword.VerificarPassword(
                    password,
                    hashGuardado);

            if (!passwordCorrecto)
            {
                return null;
            }

            // Verificar que tenga un rol permitido
            string rol =
                Convert.ToString(usuario["Rol"]);

            if (rol != "Organizador" &&
                rol != "Participante")
            {
                return null;
            }

            // Credenciales válidas
            return usuario;
        }

        public bool EsOrganizador(string rol)
        {
            return rol == "Organizador";
        }

        public bool EsParticipante(string rol)
        {
            return rol == "Participante";
        }
    }
}
