
using System;
using System.Data;
using System.Web.UI;
using UniCampus.Web.Views;
using UniCampus.Web.Presenters;

namespace UniCampus.Web
{
    public partial class Estadisticas : Page, IEstadisticaView
    {
        private EstadisticaPresenter presenter;

        // ==========================================
        // DATOS DEL USUARIO CONECTADO
        // ==========================================

        private int UsuarioActualId
        {
            get
            {
                int usuarioId = 0;

                if (Session["UsuarioId"] != null)
                {
                    int.TryParse(
                        Session["UsuarioId"].ToString(),
                        out usuarioId);
                }

                return usuarioId;
            }
        }

        private string RolActual
        {
            get
            {
                return Convert.ToString(
                    Session["RolUsuario"]);
            }
        }

        private bool EsDocente
        {
            get
            {
                return RolActual == "Organizador";
            }
        }

        private bool EsEstudiante
        {
            get
            {
                return RolActual == "Participante";
            }
        }

        // ==========================================
        // VALIDAR ACCESO
        // ==========================================

        private bool UsuarioAutorizado()
        {
            if (!Request.IsAuthenticated ||
                UsuarioActualId <= 0 ||
                Session["RolUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return false;
            }

            if (!EsDocente && !EsEstudiante)
            {
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return false;
            }

            return true;
        }

        // ==========================================
        // INICIALIZAR PRESENTER
        // ==========================================

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            presenter = new EstadisticaPresenter(this);
        }

        // ==========================================
        // CARGAR PAGINA
        // ==========================================

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            ConfigurarVistaSegunRol();

            if (!IsPostBack)
            {
                CargarEstadisticas();
            }
        }

        // ==========================================
        // CONFIGURAR ESTADISTICAS SEGUN EL ROL
        // ==========================================

        private void ConfigurarVistaSegunRol()
        {
            // LOS INDICADORES ADMINISTRATIVOS
            // SOLO SE MUESTRAN A LOS DOCENTES

            tarjetaEstudiantesActivos.Visible = EsDocente;

            tarjetaEstudiantesInactivos.Visible = EsDocente;

            // PERSONALIZAR TITULOS PARA ESTUDIANTES

            if (EsEstudiante)
            {
                lblTituloEstadisticas.Text =
                    "Mis estadísticas";

                lblSubtituloEstadisticas.Text =
                    "Consulta tus actividades, inscripciones " +
                    "y reseñas registradas en UniCampus.";

                lblTituloInscripciones.Text =
                    "Mis actividades inscritas";

                lblTituloEstados.Text =
                    "Estado de mis actividades";

                lblTituloCalificaciones.Text =
                    "Mis calificaciones por actividad";
            }
            else
            {
                lblTituloEstadisticas.Text =
                    "Panel de estadísticas";

                lblSubtituloEstadisticas.Text =
                    "Resumen general de las actividades " +
                    "académicas de UniCampus.";

                lblTituloInscripciones.Text =
                    "Inscripciones y cupos por actividad";

                lblTituloEstados.Text =
                    "Actividades por estado";

                lblTituloCalificaciones.Text =
                    "Calificaciones por actividad";
            }
        }

        // ==========================================
        // CONSULTAR DATOS DESDE EL PRESENTER
        // ==========================================

        private void CargarEstadisticas()
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            lblMensaje.Text = "";

            if (presenter == null)
            {
                presenter = new EstadisticaPresenter(this);
            }

            // ENVIAR EL ROL Y EL ID DEL USUARIO
            presenter.CargarEstadisticas(
                RolActual,
                UsuarioActualId);
        }

        // ==========================================
        // MOSTRAR RESUMEN DE ESTADISTICAS
        // ==========================================

        public DataTable ResumenGeneral
        {
            set
            {
                if (value != null && value.Rows.Count > 0)
                {
                    DataRow fila = value.Rows[0];

                    // TOTAL DE ACTIVIDADES
                    lblTotalActividades.Text =
                        fila["TotalActividades"].ToString();

                    // ACTIVIDADES PUBLICADAS
                    lblActividadesPublicadas.Text =
                        fila["ActividadesPublicadas"].ToString();

                    // INSCRIPCIONES ACTIVAS
                    lblInscripcionesActivas.Text =
                        fila["InscripcionesActivas"].ToString();

                    // INSCRIPCIONES CANCELADAS
                    lblInscripcionesCanceladas.Text =
                        fila["InscripcionesCanceladas"].ToString();

                    // TOTAL DE RESENAS
                    lblTotalResenas.Text =
                        fila["TotalResenas"].ToString();

                    // PROMEDIO DE CALIFICACIONES
                    if (Convert.ToInt32(fila["TotalResenas"]) > 0 &&
                        fila["PromedioCalificacion"] != DBNull.Value)
                    {
                        decimal promedio =
                            Convert.ToDecimal(
                                fila["PromedioCalificacion"]);

                        lblPromedioCalificacion.Text =
                            promedio.ToString("N2");
                    }
                    else
                    {
                        lblPromedioCalificacion.Text =
                            "Sin reseñas";
                    }

                    // ESTUDIANTES ACTIVOS
                    if (EsDocente)
                    {
                        lblEstudiantesActivos.Text =
                            fila["EstudiantesActivos"].ToString();

                        lblEstudiantesInactivos.Text =
                            fila["EstudiantesInactivos"].ToString();
                    }
                }
            }
        }

        // ==========================================
        // MOSTRAR INSCRIPCIONES POR ACTIVIDAD
        // ==========================================

        public DataTable InscripcionesPorActividad
        {
            set
            {
                gvInscripciones.DataSource = value;
                gvInscripciones.DataBind();
            }
        }

        // ==========================================
        // MOSTRAR ACTIVIDADES POR ESTADO
        // ==========================================

        public DataTable ActividadesPorEstado
        {
            set
            {
                gvEstados.DataSource = value;
                gvEstados.DataBind();
            }
        }

        // ==========================================
        // MOSTRAR CALIFICACIONES POR ACTIVIDAD
        // ==========================================

        public DataTable CalificacionesPorActividad
        {
            set
            {
                gvCalificaciones.DataSource = value;
                gvCalificaciones.DataBind();
            }
        }

        // ==========================================
        // MOSTRAR MENSAJES
        // ==========================================

        public void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text =
                Server.HtmlEncode(mensaje);
        }

        // ==========================================
        // ACTUALIZAR ESTADISTICAS
        // ==========================================

        protected void btnActualizar_Click(
            object sender, EventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            CargarEstadisticas();
        }
    }
}
