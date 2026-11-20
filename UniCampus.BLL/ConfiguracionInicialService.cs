
using System;
using UniCampus.DAL;

namespace UniCampus.BLL
{
    public class ConfiguracionInicialService
    {
        private AutenticacionRepository repositorio =
            new AutenticacionRepository();

        public bool ConfigurarPasswordInicial(
            int usuarioId,
            string password)
        {
            // Validar el identificador del usuario
            if (usuarioId <= 0)
            {
                throw new ArgumentException(
                    "El identificador del usuario no es válido.");
            }

            // Validar la contraseña
            if (string.IsNullOrWhiteSpace(password) ||
                password.Length < 5)
            {
                throw new ArgumentException(
                    "La contraseña debe tener al menos 5 caracteres.");
            }

            // Generar el hash seguro
            string hash =
                SeguridadPassword.GenerarHash(password);

            // Guardar el hash en SQL Server
            return repositorio.ConfigurarPasswordInicial(
                usuarioId,
                hash);
        }
    }
}
