
using System;
using System.Data;
using System.Data.SqlClient;

namespace UniCampus.DAL
{
    public class UsuarioRepository
    {
        private ConexionBD conexion = new ConexionBD();

        // CONSULTAR TODOS LOS ESTUDIANTES
        public DataTable ObtenerEstudiantes()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT UsuarioId, Nombre, Correo,
                           Programa, Estado
                    FROM dbo.Usuario
                    WHERE Rol = @Rol
                    ORDER BY Nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@Rol", "Participante");

                    using (SqlDataAdapter adaptador =
                        new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }

        // CONSULTAR SOLO ESTUDIANTES ACTIVOS
        public DataTable ObtenerEstudiantesActivos()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT UsuarioId, Nombre, Correo,
                           Programa, Estado
                    FROM dbo.Usuario
                    WHERE Rol = @Rol
                      AND Estado = @Estado
                    ORDER BY Nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@Rol", "Participante");

                    cmd.Parameters.AddWithValue(
                        "@Estado", "Activo");

                    using (SqlDataAdapter adaptador =
                        new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }

        // BUSCAR UN ESTUDIANTE POR ID
        public DataRow ObtenerEstudiantePorId(int usuarioId)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT UsuarioId, Nombre, Correo,
                           Programa, Estado
                    FROM dbo.Usuario
                    WHERE UsuarioId = @UsuarioId
                      AND Rol = 'Participante'";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    using (SqlDataAdapter adaptador =
                        new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            return tabla.Rows[0];
        }

        // COMPROBAR SI EL CORREO YA EXISTE
        public bool ExisteCorreo(string correo)
        {
            return ExisteCorreoEnOtroUsuario(correo, 0);
        }

        // COMPROBAR CORREO EXCLUYENDO AL USUARIO ACTUAL
        public bool ExisteCorreoEnOtroUsuario(
            string correo, int usuarioId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT COUNT(*)
                    FROM dbo.Usuario
                    WHERE Correo = @Correo
                      AND UsuarioId <> @UsuarioId";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@Correo", SqlDbType.NVarChar, 200).Value =
                        correo;

                    cmd.Parameters.Add(
                        "@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    con.Open();

                    int cantidad =
                        Convert.ToInt32(cmd.ExecuteScalar());

                    return cantidad > 0;
                }
            }
        }

        // REGISTRO ANTERIOR PARA COMPATIBILIDAD
        // SE CONSERVA TEMPORALMENTE HASTA
        // ACTUALIZAR USUARIOSERVICE
        public void RegistrarEstudiante(
            string nombre,
            string correo,
            string programa)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    INSERT INTO dbo.Usuario
                        (Nombre, Correo, Programa, Rol, Estado)
                    VALUES
                        (@Nombre, @Correo, @Programa,
                         @Rol, @Estado)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@Nombre", SqlDbType.NVarChar, 100).Value =
                        nombre;

                    cmd.Parameters.Add(
                        "@Correo", SqlDbType.NVarChar, 200).Value =
                        correo;

                    cmd.Parameters.Add(
                        "@Programa", SqlDbType.NVarChar, 150).Value =
                        programa;

                    cmd.Parameters.AddWithValue(
                        "@Rol", "Participante");

                    cmd.Parameters.AddWithValue(
                        "@Estado", "Activo");

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // REGISTRAR ESTUDIANTE CON CONTRASEÑA
        public void RegistrarEstudiante(
            string nombre,
            string correo,
            string programa,
            string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new Exception(
                    "Debe proporcionar una contraseña válida.");
            }

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    INSERT INTO dbo.Usuario
                        (Nombre, Correo, Programa,
                         Rol, Estado, PasswordHash)
                    VALUES
                        (@Nombre, @Correo, @Programa,
                         @Rol, @Estado, @PasswordHash)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@Nombre", SqlDbType.NVarChar, 100).Value =
                        nombre;

                    cmd.Parameters.Add(
                        "@Correo", SqlDbType.NVarChar, 200).Value =
                        correo;

                    cmd.Parameters.Add(
                        "@Programa", SqlDbType.NVarChar, 150).Value =
                        programa;

                    cmd.Parameters.Add(
                        "@Rol", SqlDbType.NVarChar, 30).Value =
                        "Participante";

                    cmd.Parameters.Add(
                        "@Estado", SqlDbType.NVarChar, 20).Value =
                        "Activo";

                    cmd.Parameters.Add(
                        "@PasswordHash", SqlDbType.NVarChar, 255).Value =
                        passwordHash;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // MODIFICAR DATOS DEL ESTUDIANTE
        public void ModificarEstudiante(
            int usuarioId,
            string nombre,
            string correo,
            string programa)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    UPDATE dbo.Usuario
                    SET Nombre = @Nombre,
                        Correo = @Correo,
                        Programa = @Programa
                    WHERE UsuarioId = @UsuarioId
                      AND Rol = 'Participante'";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    cmd.Parameters.Add(
                        "@Nombre", SqlDbType.NVarChar, 100).Value =
                        nombre;

                    cmd.Parameters.Add(
                        "@Correo", SqlDbType.NVarChar, 200).Value =
                        correo;

                    cmd.Parameters.Add(
                        "@Programa", SqlDbType.NVarChar, 150).Value =
                        programa;

                    con.Open();

                    int filas = cmd.ExecuteNonQuery();

                    if (filas == 0)
                    {
                        throw new Exception(
                            "No se encontró el estudiante que desea modificar.");
                    }
                }
            }
        }

        // RESTABLECER CONTRASEÑA DEL ESTUDIANTE
        public void RestablecerPassword(
            int usuarioId,
            string nuevoPasswordHash)
        {
            if (usuarioId <= 0)
            {
                throw new Exception(
                    "El ID del estudiante no es válido.");
            }

            if (string.IsNullOrWhiteSpace(nuevoPasswordHash))
            {
                throw new Exception(
                    "La nueva contraseña no es válida.");
            }

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    UPDATE dbo.Usuario
                    SET PasswordHash = @PasswordHash
                    WHERE UsuarioId = @UsuarioId
                      AND Rol = 'Participante'";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    cmd.Parameters.Add(
                        "@PasswordHash", SqlDbType.NVarChar, 255).Value =
                        nuevoPasswordHash;

                    con.Open();

                    int filas = cmd.ExecuteNonQuery();

                    if (filas == 0)
                    {
                        throw new Exception(
                            "No se encontró el estudiante para restablecer su contraseña.");
                    }
                }
            }
        }

        // DESACTIVAR O REACTIVAR ESTUDIANTE
        public void CambiarEstadoEstudiante(
            int usuarioId, string nuevoEstado)
        {
            if (nuevoEstado != "Activo" &&
                nuevoEstado != "Inactivo")
            {
                throw new Exception(
                    "El estado del estudiante no es válido.");
            }

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = @"
                    UPDATE dbo.Usuario
                    SET Estado = @Estado
                    WHERE UsuarioId = @UsuarioId
                      AND Rol = 'Participante'";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    cmd.Parameters.Add(
                        "@Estado", SqlDbType.NVarChar, 20).Value =
                        nuevoEstado;

                    con.Open();

                    int filas = cmd.ExecuteNonQuery();

                    if (filas == 0)
                    {
                        throw new Exception(
                            "No se encontró el estudiante.");
                    }
                }
            }
        }
    }
}
