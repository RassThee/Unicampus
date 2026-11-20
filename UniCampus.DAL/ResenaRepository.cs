
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniCampus.Entities;

namespace UniCampus.DAL
{
    public class ResenaRepository
    {
        // CONSULTAR RESENAS CON NOMBRES
        public List<Resena> ObtenerResenas()
        {
            List<Resena> lista = new List<Resena>();

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql =
                    @"SELECT
                          r.ResenaId,
                          r.ActividadId,
                          a.Titulo AS NombreActividad,
                          r.UsuarioId,
                          u.Nombre AS NombreEstudiante,
                          r.Calificacion,
                          r.Comentario,
                          r.FechaCreacion
                      FROM dbo.Resena r
                      INNER JOIN dbo.ActividadAcademica a
                          ON r.ActividadId = a.ActividadId
                      INNER JOIN dbo.Usuario u
                          ON r.UsuarioId = u.UsuarioId
                      ORDER BY r.ResenaId";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Resena resena = new Resena();

                            resena.ResenaId =
                                Convert.ToInt32(dr["ResenaId"]);

                            resena.ActividadId =
                                Convert.ToInt32(dr["ActividadId"]);

                            resena.NombreActividad =
                                dr["NombreActividad"].ToString();

                            resena.UsuarioId =
                                Convert.ToInt32(dr["UsuarioId"]);

                            resena.NombreEstudiante =
                                dr["NombreEstudiante"].ToString();

                            resena.Calificacion =
                                Convert.ToByte(dr["Calificacion"]);

                            resena.Comentario =
                                dr["Comentario"] == DBNull.Value
                                ? ""
                                : dr["Comentario"].ToString();

                            resena.FechaCreacion =
                                Convert.ToDateTime(dr["FechaCreacion"]);

                            lista.Add(resena);
                        }
                    }
                }
            }

            return lista;
        }

        // GUARDAR RESENA CON VALIDACIONES
        public void GuardarResena(Resena resena)
        {
            if (resena == null)
            {
                throw new Exception(
                    "Debe proporcionar los datos de la reseña.");
            }

            // VALIDAR ACTIVIDAD Y USUARIO
            if (resena.ActividadId <= 0 ||
                resena.UsuarioId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una actividad y un usuario válidos.");
            }

            // VALIDAR CALIFICACION
            if (resena.Calificacion < 1 ||
                resena.Calificacion > 5)
            {
                throw new Exception(
                    "La calificación debe estar entre 1 y 5.");
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
                        // VERIFICAR INSCRIPCION ACTIVA
                        string sqlInscripcion =
                            @"SELECT COUNT(*)
                              FROM dbo.Inscripcion
                              WHERE ActividadId = @ActividadId
                                AND UsuarioId = @UsuarioId
                                AND Estado = 'Activa'";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlInscripcion, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ActividadId",
                                SqlDbType.Int).Value =
                                resena.ActividadId;

                            cmd.Parameters.Add(
                                "@UsuarioId",
                                SqlDbType.Int).Value =
                                resena.UsuarioId;

                            int cantidad = Convert.ToInt32(
                                cmd.ExecuteScalar());

                            if (cantidad == 0)
                            {
                                throw new Exception(
                                    "El usuario no tiene una inscripción activa en esta actividad.");
                            }
                        }

                        // VERIFICAR QUE NO EXISTA OTRA RESENA
                        string sqlDuplicada =
                            @"SELECT COUNT(*)
                              FROM dbo.Resena
                              WHERE ActividadId = @ActividadId
                                AND UsuarioId = @UsuarioId";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlDuplicada, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ActividadId",
                                SqlDbType.Int).Value =
                                resena.ActividadId;

                            cmd.Parameters.Add(
                                "@UsuarioId",
                                SqlDbType.Int).Value =
                                resena.UsuarioId;

                            int cantidad = Convert.ToInt32(
                                cmd.ExecuteScalar());

                            if (cantidad > 0)
                            {
                                throw new Exception(
                                    "El usuario ya registró una reseña para esta actividad.");
                            }
                        }

                        // REGISTRAR RESENA
                        string sqlGuardar =
                            @"INSERT INTO dbo.Resena
                              (ActividadId,
                               UsuarioId,
                               Calificacion,
                               Comentario,
                               FechaCreacion)
                              VALUES
                              (@ActividadId,
                               @UsuarioId,
                               @Calificacion,
                               @Comentario,
                               @FechaCreacion)";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlGuardar, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ActividadId",
                                SqlDbType.Int).Value =
                                resena.ActividadId;

                            cmd.Parameters.Add(
                                "@UsuarioId",
                                SqlDbType.Int).Value =
                                resena.UsuarioId;

                            cmd.Parameters.Add(
                                "@Calificacion",
                                SqlDbType.TinyInt).Value =
                                resena.Calificacion;

                            cmd.Parameters.Add(
                                "@Comentario",
                                SqlDbType.NVarChar, -1).Value =
                                string.IsNullOrWhiteSpace(resena.Comentario)
                                ? (object)DBNull.Value
                                : resena.Comentario.Trim();

                            cmd.Parameters.Add(
                                "@FechaCreacion",
                                SqlDbType.DateTime2).Value =
                                DateTime.Now;

                            cmd.ExecuteNonQuery();
                        }

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

        // MODIFICAR CALIFICACION Y COMENTARIO
        public void ModificarResena(Resena resena)
        {
            // VALIDAR DATOS
            if (resena == null)
            {
                throw new Exception(
                    "Debe proporcionar los datos de la reseña.");
            }

            if (resena.ResenaId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una reseña válida.");
            }

            if (resena.Calificacion < 1 ||
                resena.Calificacion > 5)
            {
                throw new Exception(
                    "La calificación debe estar entre 1 y 5.");
            }

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql =
                    @"UPDATE dbo.Resena
                      SET
                          Calificacion = @Calificacion,
                          Comentario = @Comentario
                      WHERE ResenaId = @ResenaId";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add(
                        "@ResenaId",
                        SqlDbType.Int).Value =
                        resena.ResenaId;

                    cmd.Parameters.Add(
                        "@Calificacion",
                        SqlDbType.TinyInt).Value =
                        resena.Calificacion;

                    cmd.Parameters.Add(
                        "@Comentario",
                        SqlDbType.NVarChar, -1).Value =
                        string.IsNullOrWhiteSpace(resena.Comentario)
                        ? (object)DBNull.Value
                        : resena.Comentario.Trim();

                    cn.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new Exception(
                            "La reseña no existe o no se pudo modificar.");
                    }
                }
            }
        }

        // ELIMINAR RESENA
        public void EliminarResena(int resenaId)
        {
            // VALIDAR ID
            if (resenaId <= 0)
            {
                throw new Exception(
                    "Debe seleccionar una reseña válida.");
            }

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql =
                    @"DELETE FROM dbo.Resena
                      WHERE ResenaId = @ResenaId";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add(
                        "@ResenaId",
                        SqlDbType.Int).Value = resenaId;

                    cn.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new Exception(
                            "La reseña no existe o ya fue eliminada.");
                    }
                }
            }
        }
    }
}
