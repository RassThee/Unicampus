
using System;
using System.Data;
using System.Data.SqlClient;

namespace UniCampus.DAL
{
    public class EstadisticaRepository
    {
        // ==========================================
        // ESTADISTICAS GENERALES PARA DOCENTES
        // ==========================================

        // OBTENER ESTADISTICAS GENERALES
        public DataTable ObtenerResumenGeneral()
        {
            DataTable tabla = new DataTable();
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        (SELECT COUNT(*)
                         FROM dbo.ActividadAcademica)
                         AS TotalActividades,

                        (SELECT COUNT(*)
                         FROM dbo.ActividadAcademica
                         WHERE Estado = 'Publicada')
                         AS ActividadesPublicadas,

                        (SELECT COUNT(*)
                         FROM dbo.Inscripcion
                         WHERE Estado = 'Activa')
                         AS InscripcionesActivas,

                        (SELECT COUNT(*)
                         FROM dbo.Inscripcion
                         WHERE Estado = 'Cancelada')
                         AS InscripcionesCanceladas,

                        (SELECT COUNT(*)
                         FROM dbo.Resena)
                         AS TotalResenas,

                        (SELECT ISNULL(
                            AVG(CAST(Calificacion AS DECIMAL(10,2))),
                            0)
                         FROM dbo.Resena)
                         AS PromedioCalificacion,

                        (SELECT COUNT(*)
                         FROM dbo.Usuario
                         WHERE Rol = 'Participante'
                           AND Estado = 'Activo')
                         AS EstudiantesActivos,

                        (SELECT COUNT(*)
                         FROM dbo.Usuario
                         WHERE Rol = 'Participante'
                           AND Estado = 'Inactivo')
                         AS EstudiantesInactivos";

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

        // OBTENER INSCRIPCIONES Y CUPOS POR ACTIVIDAD
        public DataTable ObtenerInscripcionesPorActividad()
        {
            DataTable tabla = new DataTable();
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        a.ActividadId,
                        a.Titulo,
                        a.Cupos,

                        COUNT(i.InscripcionId)
                            AS InscripcionesActivas,

                        a.Cupos - COUNT(i.InscripcionId)
                            AS CuposDisponibles

                    FROM dbo.ActividadAcademica a

                    LEFT JOIN dbo.Inscripcion i
                        ON a.ActividadId = i.ActividadId
                        AND i.Estado = 'Activa'

                    GROUP BY
                        a.ActividadId,
                        a.Titulo,
                        a.Cupos

                    ORDER BY a.Titulo";

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

        // OBTENER ACTIVIDADES AGRUPADAS POR ESTADO
        public DataTable ObtenerActividadesPorEstado()
        {
            DataTable tabla = new DataTable();
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        Estado,
                        COUNT(*) AS Cantidad

                    FROM dbo.ActividadAcademica

                    GROUP BY Estado

                    ORDER BY Estado";

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

        // OBTENER CALIFICACIONES POR ACTIVIDAD
        public DataTable ObtenerCalificacionesPorActividad()
        {
            DataTable tabla = new DataTable();
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        a.ActividadId,
                        a.Titulo,

                        COUNT(r.ResenaId)
                            AS TotalResenas,

                        ISNULL(
                            AVG(CAST(r.Calificacion AS DECIMAL(10,2))),
                            0
                        ) AS PromedioCalificacion

                    FROM dbo.ActividadAcademica a

                    LEFT JOIN dbo.Resena r
                        ON a.ActividadId = r.ActividadId

                    GROUP BY
                        a.ActividadId,
                        a.Titulo

                    ORDER BY a.Titulo";

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

        // ==========================================
        // ESTADISTICAS PERSONALES PARA ESTUDIANTES
        // ==========================================

        // OBTENER RESUMEN DEL ESTUDIANTE
        public DataTable ObtenerResumenEstudiante(int usuarioId)
        {
            DataTable tabla = new DataTable();
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        (SELECT COUNT(*)
                         FROM dbo.ActividadAcademica
                         WHERE Estado = 'Publicada')
                         AS TotalActividades,

                        (SELECT COUNT(*)
                         FROM dbo.ActividadAcademica
                         WHERE Estado = 'Publicada')
                         AS ActividadesPublicadas,

                        (SELECT COUNT(*)
                         FROM dbo.Inscripcion
                         WHERE UsuarioId = @UsuarioId
                           AND Estado = 'Activa')
                         AS InscripcionesActivas,

                        (SELECT COUNT(*)
                         FROM dbo.Inscripcion
                         WHERE UsuarioId = @UsuarioId
                           AND Estado = 'Cancelada')
                         AS InscripcionesCanceladas,

                        (SELECT COUNT(*)
                         FROM dbo.Resena
                         WHERE UsuarioId = @UsuarioId)
                         AS TotalResenas,

                        (SELECT ISNULL(
                            AVG(CAST(Calificacion AS DECIMAL(10,2))),
                            0)
                         FROM dbo.Resena
                         WHERE UsuarioId = @UsuarioId)
                         AS PromedioCalificacion,

                        0 AS EstudiantesActivos,
                        0 AS EstudiantesInactivos";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        tabla.Load(dr);
                    }
                }
            }

            return tabla;
        }

        // OBTENER ACTIVIDADES EN LAS QUE ESTA INSCRITO
        public DataTable ObtenerInscripcionesEstudiante(int usuarioId)
        {
            DataTable tabla = new DataTable();
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        a.ActividadId,
                        a.Titulo,
                        a.Cupos,

                        COUNT(i.InscripcionId)
                            AS InscripcionesActivas,

                        a.Cupos -
                        (SELECT COUNT(*)
                         FROM dbo.Inscripcion ia
                         WHERE ia.ActividadId = a.ActividadId
                           AND ia.Estado = 'Activa')
                            AS CuposDisponibles

                    FROM dbo.ActividadAcademica a

                    INNER JOIN dbo.Inscripcion i
                        ON a.ActividadId = i.ActividadId
                        AND i.UsuarioId = @UsuarioId
                        AND i.Estado = 'Activa'

                    GROUP BY
                        a.ActividadId,
                        a.Titulo,
                        a.Cupos

                    ORDER BY a.Titulo";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        tabla.Load(dr);
                    }
                }
            }

            return tabla;
        }

        // OBTENER ESTADOS DE LAS ACTIVIDADES DEL ESTUDIANTE
        public DataTable ObtenerActividadesPorEstadoEstudiante(int usuarioId)
        {
            DataTable tabla = new DataTable();
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        a.Estado,
                        COUNT(*) AS Cantidad

                    FROM dbo.ActividadAcademica a

                    WHERE EXISTS
                    (
                        SELECT 1
                        FROM dbo.Inscripcion i
                        WHERE i.ActividadId = a.ActividadId
                          AND i.UsuarioId = @UsuarioId
                          AND i.Estado = 'Activa'
                    )

                    GROUP BY a.Estado

                    ORDER BY a.Estado";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        tabla.Load(dr);
                    }
                }
            }

            return tabla;
        }

        // OBTENER CALIFICACIONES REALIZADAS POR EL ESTUDIANTE
        public DataTable ObtenerCalificacionesEstudiante(int usuarioId)
        {
            DataTable tabla = new DataTable();
            ConexionBD conexion = new ConexionBD();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string sql = @"
                    SELECT
                        a.ActividadId,
                        a.Titulo,

                        COUNT(r.ResenaId)
                            AS TotalResenas,

                        ISNULL(
                            AVG(CAST(r.Calificacion AS DECIMAL(10,2))),
                            0
                        ) AS PromedioCalificacion

                    FROM dbo.ActividadAcademica a

                    INNER JOIN dbo.Resena r
                        ON a.ActividadId = r.ActividadId
                        AND r.UsuarioId = @UsuarioId

                    GROUP BY
                        a.ActividadId,
                        a.Titulo

                    ORDER BY a.Titulo";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value =
                        usuarioId;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        tabla.Load(dr);
                    }
                }
            }

            return tabla;
        }
    }
}
