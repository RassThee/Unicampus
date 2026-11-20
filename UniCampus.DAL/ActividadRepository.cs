
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniCampus.Entities;

namespace UniCampus.DAL
{
    public class ActividadRepository
    {
        // CONSULTAR ACTIVIDADES
        public List<ActividadAcademica> ObtenerActividades()
        {
            List<ActividadAcademica> lista =
                new List<ActividadAcademica>();

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"SELECT ActividadId, Titulo,
                               Descripcion, TipoId, FechaInicio,
                               FechaFin, HoraInicio, HoraFin,
                               Modalidad, Lugar, Cupos, Estado
                               FROM dbo.ActividadAcademica";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ActividadAcademica actividad =
                                new ActividadAcademica();

                            actividad.ActividadId =
                                Convert.ToInt32(dr["ActividadId"]);

                            actividad.Titulo =
                                dr["Titulo"].ToString();

                            actividad.Descripcion =
                                dr["Descripcion"].ToString();

                            actividad.TipoId =
                                Convert.ToInt32(dr["TipoId"]);

                            actividad.FechaInicio =
                                Convert.ToDateTime(dr["FechaInicio"]);

                            actividad.FechaFin =
                                Convert.ToDateTime(dr["FechaFin"]);

                            actividad.HoraInicio =
                                (TimeSpan)dr["HoraInicio"];

                            actividad.HoraFin =
                                (TimeSpan)dr["HoraFin"];

                            actividad.Modalidad =
                                dr["Modalidad"].ToString();

                            actividad.Lugar =
                                dr["Lugar"].ToString();

                            actividad.Cupos =
                                Convert.ToInt32(dr["Cupos"]);

                            actividad.Estado =
                                dr["Estado"].ToString();

                            lista.Add(actividad);
                        }
                    }
                }
            }

            return lista;
        }

        // BUSCAR ACTIVIDADES POR NOMBRE
        public List<ActividadAcademica> BuscarActividades(string nombre)
        {
            List<ActividadAcademica> lista =
                new List<ActividadAcademica>();

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql =
                    @"SELECT ActividadId, Titulo,
                             Descripcion, TipoId, FechaInicio,
                             FechaFin, HoraInicio, HoraFin,
                             Modalidad, Lugar, Cupos, Estado
                      FROM dbo.ActividadAcademica
                      WHERE Titulo LIKE @Nombre
                      ORDER BY Titulo";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    string texto = nombre == null
                        ? ""
                        : nombre.Trim();

                    cmd.Parameters.Add(
                        "@Nombre",
                        SqlDbType.NVarChar, 4000).Value =
                        "%" + texto.Replace("[", "[[]")
                                   .Replace("%", "[%]")
                                   .Replace("_", "[_]")
                                   + "%";

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ActividadAcademica actividad =
                                new ActividadAcademica();

                            actividad.ActividadId =
                                Convert.ToInt32(dr["ActividadId"]);

                            actividad.Titulo =
                                dr["Titulo"].ToString();

                            actividad.Descripcion =
                                dr["Descripcion"].ToString();

                            actividad.TipoId =
                                Convert.ToInt32(dr["TipoId"]);

                            actividad.FechaInicio =
                                Convert.ToDateTime(dr["FechaInicio"]);

                            actividad.FechaFin =
                                Convert.ToDateTime(dr["FechaFin"]);

                            actividad.HoraInicio =
                                (TimeSpan)dr["HoraInicio"];

                            actividad.HoraFin =
                                (TimeSpan)dr["HoraFin"];

                            actividad.Modalidad =
                                dr["Modalidad"].ToString();

                            actividad.Lugar =
                                dr["Lugar"].ToString();

                            actividad.Cupos =
                                Convert.ToInt32(dr["Cupos"]);

                            actividad.Estado =
                                dr["Estado"].ToString();

                            lista.Add(actividad);
                        }
                    }
                }
            }

            return lista;
        }

        // FILTRAR ACTIVIDADES ACADEMICAS
        public List<ActividadAcademica> FiltrarActividades(
            string nombre, DateTime? fechaInicio,
            int tipoId, string estado)
        {
            List<ActividadAcademica> lista =
                new List<ActividadAcademica>();

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql =
                    @"SELECT ActividadId, Titulo,
                             Descripcion, TipoId,
                             FechaInicio, FechaFin,
                             HoraInicio, HoraFin,
                             Modalidad, Lugar, Cupos, Estado
                      FROM dbo.ActividadAcademica
                      WHERE 1 = 1";

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;

                    // FILTRAR POR NOMBRE
                    if (!string.IsNullOrWhiteSpace(nombre))
                    {
                        sql += " AND Titulo LIKE @Nombre";

                        string texto = nombre.Trim();

                        texto = texto.Replace("[", "[[]")
                                     .Replace("%", "[%]")
                                     .Replace("_", "[_]");

                        cmd.Parameters.Add(
                            "@Nombre", SqlDbType.NVarChar, 4000
                        ).Value = "%" + texto + "%";
                    }

                    // FILTRAR POR FECHA DE INICIO
                    if (fechaInicio.HasValue)
                    {
                        sql +=
                            " AND FechaInicio >= @FechaInicio" +
                            " AND FechaInicio < @FechaSiguiente";

                        cmd.Parameters.Add(
                            "@FechaInicio", SqlDbType.DateTime
                        ).Value = fechaInicio.Value.Date;

                        cmd.Parameters.Add(
                            "@FechaSiguiente", SqlDbType.DateTime
                        ).Value = fechaInicio.Value.Date.AddDays(1);
                    }

                    // FILTRAR POR TIPO DE ACTIVIDAD
                    if (tipoId > 0)
                    {
                        sql += " AND TipoId = @TipoId";

                        cmd.Parameters.Add(
                            "@TipoId", SqlDbType.Int
                        ).Value = tipoId;
                    }

                    // FILTRAR POR ESTADO
                    if (!string.IsNullOrWhiteSpace(estado))
                    {
                        sql += " AND Estado = @Estado";

                        cmd.Parameters.Add(
                            "@Estado", SqlDbType.NVarChar, 30
                        ).Value = estado.Trim();
                    }

                    sql += " ORDER BY FechaInicio, Titulo";

                    cmd.CommandText = sql;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ActividadAcademica actividad =
                                new ActividadAcademica();

                            actividad.ActividadId =
                                Convert.ToInt32(dr["ActividadId"]);

                            actividad.Titulo =
                                dr["Titulo"].ToString();

                            actividad.Descripcion =
                                dr["Descripcion"].ToString();

                            actividad.TipoId =
                                Convert.ToInt32(dr["TipoId"]);

                            actividad.FechaInicio =
                                Convert.ToDateTime(dr["FechaInicio"]);

                            actividad.FechaFin =
                                Convert.ToDateTime(dr["FechaFin"]);

                            actividad.HoraInicio =
                                (TimeSpan)dr["HoraInicio"];

                            actividad.HoraFin =
                                (TimeSpan)dr["HoraFin"];

                            actividad.Modalidad =
                                dr["Modalidad"].ToString();

                            actividad.Lugar =
                                dr["Lugar"].ToString();

                            actividad.Cupos =
                                Convert.ToInt32(dr["Cupos"]);

                            actividad.Estado =
                                dr["Estado"].ToString();

                            lista.Add(actividad);
                        }
                    }
                }
            }

            return lista;
        }

        // GUARDAR ACTIVIDAD
        public void GuardarActividad(ActividadAcademica actividad)
        {
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"INSERT INTO dbo.ActividadAcademica
                               (Titulo, Descripcion, TipoId,
                                FechaInicio, FechaFin,
                                HoraInicio, HoraFin,
                                Modalidad, Lugar, Cupos, Estado)
                               VALUES
                               (@Titulo, @Descripcion, @TipoId,
                                @FechaInicio, @FechaFin,
                                @HoraInicio, @HoraFin,
                                @Modalidad, @Lugar, @Cupos, @Estado)";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@Titulo", actividad.Titulo);

                    cmd.Parameters.AddWithValue(
                        "@Descripcion",
                        (object)actividad.Descripcion ?? DBNull.Value);

                    cmd.Parameters.AddWithValue(
                        "@TipoId", actividad.TipoId);

                    cmd.Parameters.AddWithValue(
                        "@FechaInicio", actividad.FechaInicio);

                    cmd.Parameters.AddWithValue(
                        "@FechaFin", actividad.FechaFin);

                    cmd.Parameters.AddWithValue(
                        "@HoraInicio", actividad.HoraInicio);

                    cmd.Parameters.AddWithValue(
                        "@HoraFin", actividad.HoraFin);

                    cmd.Parameters.AddWithValue(
                        "@Modalidad", actividad.Modalidad);

                    cmd.Parameters.AddWithValue(
                        "@Lugar",
                        (object)actividad.Lugar ?? DBNull.Value);

                    cmd.Parameters.AddWithValue(
                        "@Cupos", actividad.Cupos);

                    cmd.Parameters.AddWithValue(
                        "@Estado", actividad.Estado);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // CONSULTAR TIPOS DE ACTIVIDAD
        public DataTable ObtenerTiposActividad()
        {
            DataTable tabla = new DataTable();

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"SELECT TipoId, Nombre
                               FROM dbo.TipoActividad
                               ORDER BY Nombre";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        tabla.Load(dr);
                    }
                }
            }

            return tabla;
        }

        // ACTUALIZAR ACTIVIDAD
        public bool ActualizarActividad(ActividadAcademica actividad)
        {
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"UPDATE dbo.ActividadAcademica
                               SET Titulo = @Titulo,
                                   Descripcion = @Descripcion,
                                   TipoId = @TipoId,
                                   FechaInicio = @FechaInicio,
                                   FechaFin = @FechaFin,
                                   HoraInicio = @HoraInicio,
                                   HoraFin = @HoraFin,
                                   Modalidad = @Modalidad,
                                   Lugar = @Lugar,
                                   Cupos = @Cupos,
                                   Estado = @Estado
                               WHERE ActividadId = @ActividadId";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@ActividadId", actividad.ActividadId);

                    cmd.Parameters.AddWithValue(
                        "@Titulo", actividad.Titulo);

                    cmd.Parameters.AddWithValue(
                        "@Descripcion",
                        (object)actividad.Descripcion ?? DBNull.Value);

                    cmd.Parameters.AddWithValue(
                        "@TipoId", actividad.TipoId);

                    cmd.Parameters.AddWithValue(
                        "@FechaInicio", actividad.FechaInicio);

                    cmd.Parameters.AddWithValue(
                        "@FechaFin", actividad.FechaFin);

                    cmd.Parameters.AddWithValue(
                        "@HoraInicio", actividad.HoraInicio);

                    cmd.Parameters.AddWithValue(
                        "@HoraFin", actividad.HoraFin);

                    cmd.Parameters.AddWithValue(
                        "@Modalidad", actividad.Modalidad);

                    cmd.Parameters.AddWithValue(
                        "@Lugar",
                        (object)actividad.Lugar ?? DBNull.Value);

                    cmd.Parameters.AddWithValue(
                        "@Cupos", actividad.Cupos);

                    cmd.Parameters.AddWithValue(
                        "@Estado", actividad.Estado);

                    cn.Open();

                    int filasActualizadas = cmd.ExecuteNonQuery();

                    return filasActualizadas > 0;
                }
            }
        }

        // ELIMINAR ACTIVIDAD
        public bool EliminarActividad(int actividadId)
        {
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"DELETE FROM dbo.ActividadAcademica
                               WHERE ActividadId = @ActividadId";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@ActividadId", actividadId);

                    cn.Open();

                    int filasEliminadas = cmd.ExecuteNonQuery();

                    return filasEliminadas > 0;
                }
            }
        }

        // CONSULTAR CUPOS TOTALES, INSCRITOS Y DISPONIBLES
        public DataTable ObtenerCuposActividad(int actividadId)
        {
            DataTable tabla = new DataTable();

            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        a.ActividadId,
                        a.Titulo,
                        a.Cupos AS CuposTotales,

                        (
                            SELECT COUNT(*)
                            FROM dbo.Inscripcion i
                            WHERE i.ActividadId = a.ActividadId
                              AND i.Estado = 'Activa'
                        ) AS Inscritos,

                        a.Cupos -
                        (
                            SELECT COUNT(*)
                            FROM dbo.Inscripcion i
                            WHERE i.ActividadId = a.ActividadId
                              AND i.Estado = 'Activa'
                        ) AS CuposDisponibles

                    FROM dbo.ActividadAcademica a
                    WHERE a.ActividadId = @ActividadId";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add(
                        "@ActividadId", SqlDbType.Int).Value =
                        actividadId;

                    using (SqlDataAdapter adaptador =
                        new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }
    }
}
