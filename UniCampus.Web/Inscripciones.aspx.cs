
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniCampus.Entities;
using UniCampus.Web.Views;
using UniCampus.Web.Presenters;
using UniCampus.BLL;

namespace UniCampus.Web
{
    public partial class Inscripciones : Page, IInscripcionView
    {
        private InscripcionPresenter presenter;

        public Inscripciones()
        {
            presenter = new InscripcionPresenter(this);
        }

        // VERIFICAR SI EL USUARIO ES DOCENTE
        private bool EsDocente()
        {
            return Request.IsAuthenticated &&
                   Convert.ToString(Session["RolUsuario"]) ==
                   "Organizador";
        }

        // VERIFICAR QUE LA SESION SEA VALIDA
        private bool SesionValida()
        {
            string rol =
                Convert.ToString(Session["RolUsuario"]);

            int id;

            return Request.IsAuthenticated &&
                   int.TryParse(
                       Convert.ToString(Session["UsuarioId"]),
                       out id) &&
                   id > 0 &&
                   (rol == "Organizador" ||
                    rol == "Participante");
        }

        // OBTENER EL ID DEL USUARIO ACTUAL
        private int UsuarioActual()
        {
            int id;

            if (int.TryParse(
                Convert.ToString(Session["UsuarioId"]),
                out id))
            {
                return id;
            }

            return 0;
        }

        // CARGAR PAGINA
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SesionValida())
            {
                Response.Redirect(
                    "~/Login.aspx", false);

                Context.ApplicationInstance.CompleteRequest();

                return;
            }

            bool docente = EsDocente();

            // SOLO EL DOCENTE SELECCIONA ESTUDIANTES
            pnlSeleccionEstudiante.Visible = docente;

            // ESTADISTICAS PARA DOCENTES Y ESTUDIANTES
            lnkEstadisticas.Visible = true;

            // ADMINISTRACION DE ESTUDIANTES SOLO DOCENTE
            lnkEstudiantes.Visible = docente;

            if (!IsPostBack)
            {
                CargarActividades();

                if (docente)
                {
                    CargarEstudiantes();
                }

                presenter.CargarInscripciones(
                    UsuarioActual(),
                    docente);

                LimpiarCupos();
            }
        }

        // CARGAR ACTIVIDADES
        private void CargarActividades()
        {
            try
            {
                ActividadService servicio =
                    new ActividadService();

                ddlActividad.DataSource =
                    servicio.ObtenerActividades();

                ddlActividad.DataTextField = "Titulo";
                ddlActividad.DataValueField = "ActividadId";

                ddlActividad.DataBind();

                ddlActividad.Items.Insert(
                    0,
                    new ListItem(
                        "-- Seleccione una actividad --",
                        "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "Error al cargar actividades: " +
                    ex.Message);
            }
        }

        // CARGAR ESTUDIANTES ACTIVOS
        private void CargarEstudiantes()
        {
            try
            {
                UsuarioService servicio =
                    new UsuarioService();

                ddlEstudiante.DataSource =
                    servicio.ObtenerEstudiantesActivos();

                ddlEstudiante.DataTextField = "Nombre";
                ddlEstudiante.DataValueField = "UsuarioId";

                ddlEstudiante.DataBind();

                ddlEstudiante.Items.Insert(
                    0,
                    new ListItem(
                        "-- Seleccione un estudiante --",
                        "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "Error al cargar estudiantes: " +
                    ex.Message);
            }
        }

        // CAMBIO DE ACTIVIDAD
        protected void ddlActividad_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (!SesionValida())
            {
                return;
            }

            CargarCuposActividad();
        }

        // CONSULTAR CUPOS
        private void CargarCuposActividad()
        {
            int actividadId;

            if (!int.TryParse(
                ddlActividad.SelectedValue,
                out actividadId) || actividadId <= 0)
            {
                LimpiarCupos();
                return;
            }

            try
            {
                ActividadService servicio =
                    new ActividadService();

                DataTable tabla =
                    servicio.ObtenerCuposActividad(
                        actividadId);

                if (tabla.Rows.Count == 0)
                {
                    LimpiarCupos();
                    return;
                }

                DataRow fila = tabla.Rows[0];

                int cuposTotales =
                    Convert.ToInt32(fila["CuposTotales"]);

                int inscritos =
                    Convert.ToInt32(fila["Inscritos"]);

                int disponibles =
                    Convert.ToInt32(fila["CuposDisponibles"]);

                lblCuposTotales.Text =
                    cuposTotales.ToString();

                lblInscritos.Text =
                    inscritos.ToString();

                lblDisponibles.Text =
                    disponibles.ToString();

                if (disponibles <= 0)
                {
                    lblDisponibles.ForeColor =
                        System.Drawing.Color.Red;
                }
                else
                {
                    lblDisponibles.ForeColor =
                        System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                LimpiarCupos();

                MostrarMensaje(
                    "Error al consultar los cupos: " +
                    ex.Message);
            }
        }

        // LIMPIAR INFORMACION DE CUPOS
        private void LimpiarCupos()
        {
            lblCuposTotales.Text = "0";
            lblInscritos.Text = "0";
            lblDisponibles.Text = "0";

            lblDisponibles.ForeColor =
                System.Drawing.Color.Black;
        }

        // REGISTRAR INSCRIPCION
        protected void btnInscribir_Click(
            object sender,
            EventArgs e)
        {
            if (!SesionValida())
            {
                MostrarMensaje(
                    "La sesión no es válida.");

                return;
            }

            int actividadId;

            if (!int.TryParse(
                ddlActividad.SelectedValue,
                out actividadId) || actividadId <= 0)
            {
                MostrarMensaje(
                    "Debe seleccionar una actividad.");

                return;
            }

            int usuarioId = UsuarioActual();

            // DOCENTE PUEDE SELECCIONAR ESTUDIANTE
            if (EsDocente())
            {
                if (!int.TryParse(
                    ddlEstudiante.SelectedValue,
                    out usuarioId) || usuarioId <= 0)
                {
                    MostrarMensaje(
                        "Debe seleccionar un estudiante.");

                    return;
                }
            }

            // ESTUDIANTE USA SU PROPIO ID DE SESION
            if (usuarioId <= 0)
            {
                MostrarMensaje(
                    "No se pudo identificar al estudiante.");

                return;
            }

            // CREAR INSCRIPCION
            Inscripcion inscripcion =
                new Inscripcion();

            inscripcion.ActividadId = actividadId;
            inscripcion.UsuarioId = usuarioId;
            inscripcion.FechaInscripcion = DateTime.Now;
            inscripcion.Estado = "Activa";

            // GUARDAR UTILIZANDO EL PRESENTER
            bool guardado =
                presenter.GuardarInscripcion(
                    inscripcion,
                    UsuarioActual(),
                    EsDocente());

            if (guardado)
            {
                LimpiarFormulario();
            }
            else
            {
                CargarCuposActividad();
            }
        }

        // LIMPIAR FORMULARIO
        private void LimpiarFormulario()
        {
            if (ddlActividad.Items.Count > 0)
            {
                ddlActividad.SelectedIndex = 0;
            }

            if (ddlEstudiante.Items.Count > 0)
            {
                ddlEstudiante.SelectedIndex = 0;
            }

            LimpiarCupos();
        }

        // CANCELAR INSCRIPCION
        protected void gvInscripciones_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName != "CancelarInscripcion")
            {
                return;
            }

            if (!SesionValida())
            {
                MostrarMensaje(
                    "La sesión no es válida.");

                return;
            }

            int inscripcionId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out inscripcionId) || inscripcionId <= 0)
            {
                MostrarMensaje(
                    "El ID de la inscripción no es válido.");

                return;
            }

            // VALIDAR PERMISOS Y CANCELAR
            presenter.CancelarInscripcion(
                inscripcionId,
                UsuarioActual(),
                EsDocente());

            // ACTUALIZAR CUPOS
            CargarCuposActividad();
        }

        // MOSTRAR INSCRIPCIONES
        public void MostrarInscripciones(
            List<Inscripcion> inscripciones)
        {
            gvInscripciones.DataSource = inscripciones;
            gvInscripciones.DataBind();
        }

        // MOSTRAR MENSAJES
        public void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text = mensaje;
        }
    }
}
