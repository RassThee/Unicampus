
using System;
using System.Data;
using System.Data.SqlClient;

namespace UniCampus.DAL
{
    public class AutenticacionRepository
    {
        private ConexionBD conexionBD = new ConexionBD();

        // BUSCAR USUARIO POR CORREO
        public DataTable BuscarUsuarioPorCorreo(string correo)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion =
                   conexionBD.ObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        UsuarioId,
                        Nombre,
                        Correo,
                        Rol,
                        Estado,
                        PasswordHash
                    FROM dbo.Usuario
                    WHERE Correo = @Correo";

                using (SqlCommand comando =
                       new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add(
                        "@Correo",
                        SqlDbType.NVarChar,
                        200
                    ).Value = correo.Trim();

                    using (SqlDataAdapter adaptador =
                           new SqlDataAdapter(comando))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }

        // GUARDAR HASH DE CONTRASEÑA
        // SOLO PARA CUENTAS QUE AUN NO TIENEN CONTRASEÑA
        public bool ConfigurarPasswordInicial(
            int usuarioId,
            string passwordHash)
        {
            if (usuarioId <= 0 ||
                string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            using (SqlConnection conexion =
                   conexionBD.ObtenerConexion())
            {
                string consulta = @"
                    UPDATE dbo.Usuario
                    SET PasswordHash = @PasswordHash
                    WHERE UsuarioId = @UsuarioId
                      AND (PasswordHash IS NULL
                           OR PasswordHash = '')";

                using (SqlCommand comando =
                       new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add(
                        "@UsuarioId",
                        SqlDbType.Int
                    ).Value = usuarioId;

                    comando.Parameters.Add(
                        "@PasswordHash",
                        SqlDbType.NVarChar,
                        255
                    ).Value = passwordHash;

                    conexion.Open();

                    int filasAfectadas =
                        comando.ExecuteNonQuery();

                    return filasAfectadas == 1;
                }
            }
        }
    }
}
