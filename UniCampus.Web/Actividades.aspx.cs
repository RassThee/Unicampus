using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniCampus.Entities;
using UniCampus.Web.Views;
using UniCampus.Web.Presenters;

namespace UniCampus.Web
{
    public partial class Actividades : Page, IActividadView
    {
        private ActividadPresenter presenter;

        // Los nombres internos de los roles se mantienen en SQL Server.
        private bool EsDocente()
        {
            return Request.IsAuthenticated &&
                   Session["UsuarioId"] != null &&
                   Convert.ToString(Session["RolUsuario"]) == "Organizador";
        }

        private bool SesionValida()
        {
            string rol = Convert.ToString(Session["RolUsuario"]);
            return Request.IsAuthenticated &&
                   Session["UsuarioId"] != null &&
                   (rol == "Organizador" || rol == "Participante");
        }

        private bool AutorizarEdicion()
        {
            if (EsDocente())
                return true;

            MostrarMensaje("No tiene permisos para administrar actividades.");
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SesionValida())
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            presenter = new ActividadPresenter(this);

            // Controles de escritura: solo docentes.
            bool docente = EsDocente();
            pnlFormularioActividad.Visible = docente;
            btnGuardar.Visible = docente;
            btnCancelar.Visible = docente && btnCancelar.Visible;

            if (!IsPostBack)
            {
                presenter.CargarTiposActividad();
                presenter.CargarActividades();
            }
        }

        // MOSTRAR ACTIVIDADES EN LA TABLA
        public void MostrarActividades(
            List<ActividadAcademica> actividades)
        {
            gvActividades.DataSource = actividades;
            gvActividades.DataBind();

            // Ocultar acciones administrativas para estudiantes.
            if (!EsDocente())
            {
                foreach (GridViewRow fila in gvActividades.Rows)
                {
                    Button editar = fila.FindControl("btnEditar") as Button;
                    Button eliminar = fila.FindControl("btnEliminar") as Button;
                    if (editar != null) editar.Visible = false;
                    if (eliminar != null) eliminar.Visible = false;
                }
            }
        }

        // MOSTRAR TIPOS DE ACTIVIDAD
        public void MostrarTiposActividad(DataTable tipos)
        {
            // TIPOS DEL FORMULARIO
            ddlTipo.DataSource = tipos;
            ddlTipo.DataTextField = "Nombre";
            ddlTipo.DataValueField = "TipoId";
            ddlTipo.DataBind();

            ddlTipo.Items.Insert(
                0, new ListItem("Seleccione", "0"));

            // TIPOS DEL BUSCADOR
            CargarTiposFiltro(tipos);
        }

        // CARGAR TIPOS EN EL FILTRO
        private void CargarTiposFiltro(DataTable tipos)
        {
            ddlFiltroTipo.Items.Clear();

            ddlFiltroTipo.DataSource = tipos;
            ddlFiltroTipo.DataTextField = "Nombre";
            ddlFiltroTipo.DataValueField = "TipoId";
            ddlFiltroTipo.DataBind();

            ddlFiltroTipo.Items.Insert(
                0, new ListItem("Todos los tipos", "0"));

            ddlFiltroTipo.SelectedIndex = 0;
        }

        // MOSTRAR MENSAJES
        public void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text = mensaje;
        }

        // BOTON BUSCAR Y FILTRAR
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        // BOTON MOSTRAR TODAS
        protected void btnMostrarTodas_Click(
            object sender, EventArgs e)
        {
            txtBuscarActividad.Text = "";
            txtFiltroFecha.Text = "";

            ddlFiltroTipo.ClearSelection();
            ddlFiltroEstado.ClearSelection();

            ddlFiltroTipo.SelectedValue = "0";
            ddlFiltroEstado.SelectedValue = "";

            presenter.CargarActividades();

            MostrarMensaje(
                "Se muestran todas las actividades.");
        }

        // APLICAR FILTROS
        private void AplicarFiltros()
        {
            string nombre = txtBuscarActividad.Text.Trim();
            string estado = ddlFiltroEstado.SelectedValue;

            DateTime fecha;
            DateTime? fechaInicio = null;

            if (!string.IsNullOrWhiteSpace(txtFiltroFecha.Text))
            {
                if (!DateTime.TryParse(
                    txtFiltroFecha.Text, out fecha))
                {
                    MostrarMensaje(
                        "Debe seleccionar una fecha valida.");
                    return;
                }

                fechaInicio = fecha;
            }

            int tipoId;

            if (!int.TryParse(
                ddlFiltroTipo.SelectedValue, out tipoId))
            {
                MostrarMensaje(
                    "Debe seleccionar un tipo valido.");
                return;
            }

            presenter.FiltrarActividades(
                nombre, fechaInicio, tipoId, estado);
        }

        // BOTONES DE LA TABLA
        protected void gvActividades_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "VerDetalles" &&
                e.CommandName != "EditarActividad" &&
                e.CommandName != "EliminarActividad")
            {
                return;
            }

            int actividadId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out actividadId) || actividadId <= 0)
            {
                MostrarMensaje("ID de actividad no valido.");
                return;
            }

            // VER DETALLES
            if (e.CommandName == "VerDetalles")
            {
                try
                {
                    ActividadAcademica actividad =
                        ObtenerActividadSeleccionada(actividadId);

                    if (actividad == null)
                    {
                        pnlDetalles.Visible = false;

                        MostrarMensaje(
                            "No se encontro la actividad.");
                        return;
                    }

                    MostrarDetallesActividad(actividad);
                }
                catch (Exception)
                {
                    pnlDetalles.Visible = false;

                    MostrarMensaje(
                        "No fue posible consultar los detalles.");
                }

                return;
            }

            // Los comandos de edición y eliminación requieren rol docente.
            if (!AutorizarEdicion())
                return;

            // ELIMINAR ACTIVIDAD
            if (e.CommandName == "EliminarActividad")
            {
                bool eliminado =
                    presenter.EliminarActividad(actividadId);

                if (eliminado)
                {
                    if (hfActividadId.Value ==
                        actividadId.ToString())
                    {
                        LimpiarFormulario();
                    }

                    // OCULTAR DETALLES ANTERIORES
                    pnlDetalles.Visible = false;

                    ActualizarListado();
                }

                return;
            }

            // EDITAR ACTIVIDAD
            if (e.CommandName == "EditarActividad")
            {
                try
                {
                    ActividadAcademica actividad =
                        ObtenerActividadSeleccionada(actividadId);

                    if (actividad == null)
                    {
                        MostrarMensaje(
                            "No se encontro la actividad.");
                        return;
                    }

                    hfActividadId.Value =
                        actividad.ActividadId.ToString();

                    txtTitulo.Text = actividad.Titulo;
                    txtDescripcion.Text = actividad.Descripcion;

                    ListItem tipo = ddlTipo.Items.FindByValue(
                        actividad.TipoId.ToString());

                    if (tipo != null)
                    {
                        ddlTipo.SelectedValue =
                            actividad.TipoId.ToString();
                    }

                    txtFechaInicio.Text =
                        actividad.FechaInicio.ToString("yyyy-MM-dd");

                    txtFechaFin.Text =
                        actividad.FechaFin.ToString("yyyy-MM-dd");

                    txtHoraInicio.Text =
                        actividad.HoraInicio.ToString(@"hh\:mm");

                    txtHoraFin.Text =
                        actividad.HoraFin.ToString(@"hh\:mm");

                    ListItem modalidad =
                        ddlModalidad.Items.FindByValue(
                            actividad.Modalidad);

                    if (modalidad != null)
                    {
                        ddlModalidad.SelectedValue =
                            actividad.Modalidad;
                    }

                    txtLugar.Text = actividad.Lugar;
                    txtCupos.Text = actividad.Cupos.ToString();

                    ListItem estado =
                        ddlEstado.Items.FindByValue(
                            actividad.Estado);

                    if (estado != null)
                    {
                        ddlEstado.SelectedValue =
                            actividad.Estado;
                    }

                    lblTituloFormulario.Text =
                        "Modificar actividad ID: " + actividadId;

                    btnGuardar.Text = "Actualizar actividad";
                    btnCancelar.Visible = true;

                    pnlDetalles.Visible = false;

                    MostrarMensaje(
                        "Modifique los datos y presione Actualizar actividad.");
                }
                catch (Exception)
                {
                    MostrarMensaje(
                        "No fue posible cargar la actividad para editar.");
                }
            }
        }

        // BUSCAR ACTIVIDAD POR ID
        private ActividadAcademica ObtenerActividadSeleccionada(int id)
        {
            UniCampus.BLL.ActividadService servicio =
                new UniCampus.BLL.ActividadService();

            List<ActividadAcademica> actividades =
                servicio.ObtenerActividades();

            return actividades.FirstOrDefault(
                a => a.ActividadId == id);
        }

        // MOSTRAR TODOS LOS DETALLES
        private void MostrarDetallesActividad(
            ActividadAcademica actividad)
        {
            lblDetalleId.Text =
                actividad.ActividadId.ToString();

            lblDetalleTitulo.Text =
                HttpUtility.HtmlEncode(actividad.Titulo);

            lblDetalleDescripcion.Text =
                HttpUtility.HtmlEncode(actividad.Descripcion);

            // MOSTRAR NOMBRE DEL TIPO
            ListItem tipo = ddlTipo.Items.FindByValue(
                actividad.TipoId.ToString());

            if (tipo != null)
            {
                lblDetalleTipo.Text =
                    HttpUtility.HtmlEncode(tipo.Text);
            }
            else
            {
                lblDetalleTipo.Text =
                    actividad.TipoId.ToString();
            }

            lblDetalleFechaInicio.Text =
                actividad.FechaInicio.ToString("dd/MM/yyyy");

            lblDetalleFechaFin.Text =
                actividad.FechaFin.ToString("dd/MM/yyyy");

            lblDetalleHoraInicio.Text =
                actividad.HoraInicio.ToString(@"hh\:mm");

            lblDetalleHoraFin.Text =
                actividad.HoraFin.ToString(@"hh\:mm");

            lblDetalleModalidad.Text =
                HttpUtility.HtmlEncode(actividad.Modalidad);

            lblDetalleLugar.Text =
                HttpUtility.HtmlEncode(actividad.Lugar);

            lblDetalleCupos.Text =
                actividad.Cupos.ToString();

            lblDetalleEstado.Text =
                HttpUtility.HtmlEncode(actividad.Estado);

            pnlDetalles.Visible = true;

            MostrarMensaje(
                "Detalles de la actividad ID: " +
                actividad.ActividadId);
        }

        // CERRAR PANEL DE DETALLES
        protected void btnCerrarDetalles_Click(
            object sender, EventArgs e)
        {
            pnlDetalles.Visible = false;

            MostrarMensaje(
                "Panel de detalles cerrado.");
        }

        // GUARDAR O ACTUALIZAR ACTIVIDAD
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!AutorizarEdicion())
                return;

            DateTime fechaInicio;
            DateTime fechaFin;
            TimeSpan horaInicio;
            TimeSpan horaFin;
            int cupos;
            int tipoId;

            // VALIDAR FECHAS
            if (!DateTime.TryParse(
                txtFechaInicio.Text, out fechaInicio) ||
                !DateTime.TryParse(
                txtFechaFin.Text, out fechaFin))
            {
                MostrarMensaje("Debe ingresar fechas validas.");
                return;
            }

            // VALIDAR HORARIOS
            if (!TimeSpan.TryParse(
                txtHoraInicio.Text, out horaInicio) ||
                !TimeSpan.TryParse(
                txtHoraFin.Text, out horaFin))
            {
                MostrarMensaje("Debe ingresar horarios validos.");
                return;
            }

            // VALIDAR CUPOS
            if (!int.TryParse(txtCupos.Text, out cupos))
            {
                MostrarMensaje(
                    "Debe ingresar un numero valido de cupos.");
                return;
            }

            // VALIDAR TIPO
            if (!int.TryParse(
                ddlTipo.SelectedValue, out tipoId))
            {
                MostrarMensaje(
                    "Debe seleccionar un tipo valido.");
                return;
            }

            // CREAR ACTIVIDAD
            ActividadAcademica actividad =
                new ActividadAcademica();

            actividad.Titulo = txtTitulo.Text.Trim();
            actividad.Descripcion = txtDescripcion.Text.Trim();
            actividad.TipoId = tipoId;
            actividad.FechaInicio = fechaInicio;
            actividad.FechaFin = fechaFin;
            actividad.HoraInicio = horaInicio;
            actividad.HoraFin = horaFin;
            actividad.Modalidad = ddlModalidad.SelectedValue;
            actividad.Lugar = txtLugar.Text.Trim();
            actividad.Cupos = cupos;
            actividad.Estado = ddlEstado.SelectedValue;

            bool resultado;

            // ACTUALIZAR
            if (!string.IsNullOrEmpty(hfActividadId.Value))
            {
                int actividadId;

                if (!int.TryParse(
                    hfActividadId.Value, out actividadId)
                    || actividadId <= 0)
                {
                    MostrarMensaje(
                        "ID de actividad no valido.");
                    return;
                }

                actividad.ActividadId = actividadId;

                resultado =
                    presenter.ActualizarActividad(actividad);
            }
            else
            {
                // REGISTRAR
                resultado =
                    presenter.GuardarActividad(actividad);
            }

            if (resultado)
            {
                ActualizarListado();
                LimpiarFormulario();

                // EVITAR MOSTRAR DATOS DESACTUALIZADOS
                pnlDetalles.Visible = false;
            }
        }

        // CANCELAR EDICION
        protected void btnCancelar_Click(
            object sender, EventArgs e)
        {
            if (!AutorizarEdicion())
                return;

            LimpiarFormulario();

            MostrarMensaje("Edicion cancelada.");
        }

        // ACTUALIZAR TABLA CON FILTROS
        private void ActualizarListado()
        {
            AplicarFiltros();
        }

        // LIMPIAR FORMULARIO
        private void LimpiarFormulario()
        {
            hfActividadId.Value = "";

            txtTitulo.Text = "";
            txtDescripcion.Text = "";
            ddlTipo.SelectedIndex = 0;
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            txtHoraInicio.Text = "";
            txtHoraFin.Text = "";
            ddlModalidad.SelectedIndex = 0;
            txtLugar.Text = "";
            txtCupos.Text = "";
            ddlEstado.SelectedIndex = 0;

            lblTituloFormulario.Text =
                "Registrar nueva actividad";

            btnGuardar.Text = "Guardar actividad";
            btnCancelar.Visible = false;
        }
    }
}