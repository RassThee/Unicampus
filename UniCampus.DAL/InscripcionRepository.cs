
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniCampus.Entities;

namespace UniCampus.DAL
{
    public class InscripcionRepository
    {
        // CONSULTAR INSCRIPCIONES
        public List<Inscripcion> ObtenerInscripciones()
        {
            List<Inscripcion> lista =
                new List<Inscripcion>();

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        i.InscripcionId,
                        i.ActividadId,
                        a.Titulo AS NombreActividad,
                        i.UsuarioId,
                        u.Nombre AS NombreEstudiante,
                        u.Programa,
                        i.FechaInscripcion,
                        i.Estado
                    FROM dbo.Inscripcion i
                    INNER JOIN dbo.ActividadAcademica a
                        ON i.ActividadId = a.ActividadId
                    INNER JOIN dbo.Usuario u
                        ON i.UsuarioId = u.UsuarioId
                    ORDER BY i.InscripcionId";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Inscripcion inscripcion =
                                new Inscripcion();

                            inscripcion.InscripcionId =
                                Convert.ToInt32(dr["InscripcionId"]);

                            inscripcion.ActividadId =
                                Convert.ToInt32(dr["ActividadId"]);

                            inscripcion.UsuarioId =
                                Convert.ToInt32(dr["UsuarioId"]);

                            inscripcion.NombreActividad =
                                dr["NombreActividad"].ToString();

                            inscripcion.NombreEstudiante =
                                dr["NombreEstudiante"].ToString();

                            inscripcion.Programa =
                                dr["Programa"] == DBNull.Value
                                ? ""
                                : dr["Programa"].ToString();

                            inscripcion.FechaInscripcion =
                                Convert.ToDateTime(dr["FechaInscripcion"]);

                            inscripcion.Estado =
                                dr["Estado"].ToString();

                            lista.Add(inscripcion);
                        }
                    }
                }
            }

            return lista;
        }

        // CONSULTAR ESTUDIANTES INSCRITOS EN UNA ACTIVIDAD
        public DataTable ObtenerEstudiantesInscritos(int actividadId)
        {
            if (actividadId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar una actividad válida.");
            }

            DataTable tabla = new DataTable();

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT DISTINCT
                        u.UsuarioId,
                        u.Nombre
                    FROM dbo.Inscripcion i
                    INNER JOIN dbo.Usuario u
                        ON i.UsuarioId = u.UsuarioId
                    WHERE i.ActividadId = @ActividadId
                      AND i.Estado = 'Activa'
                      AND u.Estado = 'Activo'
                      AND u.Rol = 'Participante'
                    ORDER BY u.Nombre";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add(
                        "@ActividadId",
                        SqlDbType.Int).Value = actividadId;

                    using (SqlDataAdapter adaptador =
                        new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }

        // GUARDAR INSCRIPCION CON VALIDACIONES
        public void GuardarInscripcion(Inscripcion inscripcion)
        {
            if (inscripcion == null)
            {
                throw new Exception(
                    "Debe proporcionar los datos de la inscripción.");
            }

            if (inscripcion.ActividadId <= 0 ||
                inscripcion.UsuarioId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una actividad y un estudiante válidos.");
            }

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                using (SqlTransaction transaccion =
                    cn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // CONSULTAR ESTADO Y ROL DEL USUARIO
                        string sqlUsuario =
                            @"SELECT Estado, Rol
                              FROM dbo.Usuario
                              WHERE UsuarioId = @UsuarioId";

                        string estadoUsuario;
                        string rolUsuario;

                        using (SqlCommand cmd =
                            new SqlCommand(sqlUsuario, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@UsuarioId",
                                SqlDbType.Int).Value =
                                inscripcion.UsuarioId;

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (!dr.Read())
                                {
                                    throw new Exception(
                                        "El estudiante no existe.");
                                }

                                estadoUsuario =
                                    dr["Estado"].ToString();

                                rolUsuario =
                                    dr["Rol"].ToString();
                            }
                        }

                        // VERIFICAR QUE SEA ESTUDIANTE
                        if (rolUsuario != "Participante")
                        {
                            throw new Exception(
                                "El usuario no tiene el rol Participante y no puede inscribirse.");
                        }

                        // VERIFICAR QUE ESTE ACTIVO
                        if (estadoUsuario != "Activo")
                        {
                            throw new Exception(
                                "El estudiante está inactivo y no puede inscribirse.");
                        }

                        // CONSULTAR CUPOS Y ESTADO DE LA ACTIVIDAD
                        string sqlActividad =
                            @"SELECT Cupos, Estado
                              FROM dbo.ActividadAcademica
                                   WITH (UPDLOCK, HOLDLOCK)
                              WHERE ActividadId = @ActividadId";

                        int cupos;
                        string estadoActividad;

                        using (SqlCommand cmd =
                            new SqlCommand(sqlActividad, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ActividadId",
                                SqlDbType.Int).Value =
                                inscripcion.ActividadId;

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (!dr.Read())
                                {
                                    throw new Exception(
                                        "La actividad no existe.");
                                }

                                cupos =
                                    Convert.ToInt32(dr["Cupos"]);

                                estadoActividad =
                                    dr["Estado"].ToString();
                            }
                        }

                        // VERIFICAR QUE LA ACTIVIDAD ESTE PUBLICADA
                        if (estadoActividad != "Publicada")
                        {
                            throw new Exception(
                                "No se puede registrar la inscripción porque la actividad no está publicada.");
                        }

                        // VERIFICAR INSCRIPCION DUPLICADA
                        string sqlDuplicada =
                            @"SELECT COUNT(*)
                              FROM dbo.Inscripcion
                              WHERE ActividadId = @ActividadId
                                AND UsuarioId = @UsuarioId
                                AND Estado = 'Activa'";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlDuplicada, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ActividadId",
                                SqlDbType.Int).Value =
                                inscripcion.ActividadId;

                            cmd.Parameters.Add(
                                "@UsuarioId",
                                SqlDbType.Int).Value =
                                inscripcion.UsuarioId;

                            int cantidad = Convert.ToInt32(
                                cmd.ExecuteScalar());

                            if (cantidad > 0)
                            {
                                throw new Exception(
                                    "El estudiante ya está inscrito en esta actividad.");
                            }
                        }

                        // CONTAR INSCRIPCIONES ACTIVAS
                        string sqlCantidad =
                            @"SELECT COUNT(*)
                              FROM dbo.Inscripcion
                              WHERE ActividadId = @ActividadId
                                AND Estado = 'Activa'";

                        int inscritos;

                        using (SqlCommand cmd =
                            new SqlCommand(sqlCantidad, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ActividadId",
                                SqlDbType.Int).Value =
                                inscripcion.ActividadId;

                            inscritos = Convert.ToInt32(
                                cmd.ExecuteScalar());
                        }

                        // VERIFICAR CUPOS DISPONIBLES
                        if (inscritos >= cupos)
                        {
                            throw new Exception(
                                "No hay cupos disponibles para esta actividad.");
                        }

                        // REGISTRAR INSCRIPCION
                        string sqlGuardar =
                            @"INSERT INTO dbo.Inscripcion
                              (ActividadId,
                               UsuarioId,
                               FechaInscripcion,
                               Estado)
                              VALUES
                              (@ActividadId,
                               @UsuarioId,
                               @FechaInscripcion,
                               @Estado)";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlGuardar, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ActividadId",
                                SqlDbType.Int).Value =
                                inscripcion.ActividadId;

                            cmd.Parameters.Add(
                                "@UsuarioId",
                                SqlDbType.Int).Value =
                                inscripcion.UsuarioId;

                            cmd.Parameters.Add(
                                "@FechaInscripcion",
                                SqlDbType.DateTime2).Value =
                                DateTime.Now;

                            cmd.Parameters.Add(
                                "@Estado",
                                SqlDbType.NVarChar, 20).Value =
                                "Activa";

                            cmd.ExecuteNonQuery();
                        }

                        // CONFIRMAR REGISTRO
                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        // CANCELAR INSCRIPCION
        public void CancelarInscripcion(int inscripcionId)
        {
            if (inscripcionId <= 0)
            {
                throw new Exception(
                    "El ID de la inscripción no es válido.");
            }

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql =
                    @"UPDATE dbo.Inscripcion
                      SET Estado = 'Cancelada'
                      WHERE InscripcionId = @InscripcionId
                        AND Estado = 'Activa'";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add(
                        "@InscripcionId",
                        SqlDbType.Int).Value = inscripcionId;

                    cn.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new Exception(
                            "La inscripción no existe o ya está cancelada.");
                    }
                }
            }
        }
    }
}
