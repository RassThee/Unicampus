
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniCampus.BLL;
using UniCampus.Entities;
using UniCampus.Web.Presenters;
using UniCampus.Web.Views;

namespace UniCampus.Web
{
    public partial class Resenas : Page, IResenaView
    {
        private ResenaPresenter presenter;

        // ID DE LA RESENA QUE SE ESTA MODIFICANDO
        private int ResenaSeleccionadaId
        {
            get
            {
                object valor = ViewState["ResenaSeleccionadaId"];

                if (valor == null)
                {
                    return 0;
                }

                return Convert.ToInt32(valor);
            }
            set
            {
                ViewState["ResenaSeleccionadaId"] = value;
            }
        }

        // ID DEL USUARIO QUE INICIO SESION
        private int UsuarioActualId
        {
            get
            {
                int usuarioId;

                if (int.TryParse(
                    Convert.ToString(Session["UsuarioId"]),
                    out usuarioId))
                {
                    return usuarioId;
                }

                return 0;
            }
        }

        // VERIFICAR ROL ESTUDIANTE
        private bool EsEstudiante
        {
            get
            {
                return Convert.ToString(
                    Session["RolUsuario"]) == "Participante";
            }
        }

        // VERIFICAR ROL DOCENTE
        private bool EsDocente
        {
            get
            {
                return Convert.ToString(
                    Session["RolUsuario"]) == "Organizador";
            }
        }

        // VERIFICAR PERMISOS
        private bool UsuarioAutorizado()
        {
            return Request.IsAuthenticated &&
                   UsuarioActualId > 0 &&
                   (EsEstudiante || EsDocente);
        }

        // INICIALIZAR PRESENTER
        public Resenas()
        {
            presenter = new ResenaPresenter(this);
        }

        // CARGAR PAGINA
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                Response.Redirect(
                    "~/Login.aspx", false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarActividades();
                LimpiarFormulario();
                presenter.CargarResenas();
            }
        }

        // VERIFICAR INSCRIPCION ACTIVA
        private bool EstaInscrito(
            int actividadId,
            int usuarioId)
        {
            if (actividadId <= 0 || usuarioId <= 0)
            {
                return false;
            }

            InscripcionService servicio =
                new InscripcionService();

            DataTable estudiantes =
                servicio.ObtenerEstudiantesInscritos(
                    actividadId);

            foreach (DataRow fila in estudiantes.Rows)
            {
                int id;

                if (int.TryParse(
                    Convert.ToString(fila["UsuarioId"]),
                    out id) && id == usuarioId)
                {
                    return true;
                }
            }

            return false;
        }

        // CARGAR ACTIVIDADES SEGUN EL ROL
        private void CargarActividades()
        {
            ddlActividad.Items.Clear();

            try
            {
                ActividadService servicio =
                    new ActividadService();

                // EL SERVICIO DEVUELVE UNA LISTA
                List<ActividadAcademica> actividades =
                    servicio.ObtenerActividades();

                ddlActividad.Items.Add(
                    new ListItem(
                        "-- Seleccione una actividad --",
                        "0"));

                foreach (ActividadAcademica actividad
                    in actividades)
                {
                    // DOCENTE: TODAS LAS ACTIVIDADES
                    // ESTUDIANTE: SOLO DONDE ESTA INSCRITO
                    if (EsDocente ||
                        EstaInscrito(
                            actividad.ActividadId,
                            UsuarioActualId))
                    {
                        ddlActividad.Items.Add(
                            new ListItem(
                                actividad.Titulo,
                                actividad.ActividadId.ToString()));
                    }
                }

                if (EsEstudiante &&
                    ddlActividad.Items.Count == 1)
                {
                    MostrarMensaje(
                        "No tienes actividades con " +
                        "inscripciones activas.");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "Error al cargar actividades: " +
                    ex.Message);
            }
        }

        // CUANDO SE SELECCIONA UNA ACTIVIDAD
        protected void ddlActividad_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            lblMensaje.Text = "";

            if (ResenaSeleccionadaId > 0)
            {
                return;
            }

            CargarEstudiantesInscritos();
        }

        // CARGAR ESTUDIANTES INSCRITOS
        private void CargarEstudiantesInscritos()
        {
            LimpiarListaEstudiantes();

            int actividadId;

            if (!int.TryParse(
                ddlActividad.SelectedValue,
                out actividadId) || actividadId <= 0)
            {
                return;
            }

            try
            {
                InscripcionService servicio =
                    new InscripcionService();

                DataTable estudiantes =
                    servicio.ObtenerEstudiantesInscritos(
                        actividadId);

                if (EsEstudiante)
                {
                    // SOLO MOSTRAR EL ESTUDIANTE
                    // QUE INICIO SESION
                    foreach (DataRow fila in estudiantes.Rows)
                    {
                        int usuarioId;

                        if (int.TryParse(
                            Convert.ToString(fila["UsuarioId"]),
                            out usuarioId) &&
                            usuarioId == UsuarioActualId)
                        {
                            ddlEstudiante.Items.Clear();

                            ddlEstudiante.Items.Add(
                                new ListItem(
                                    Convert.ToString(
                                        fila["Nombre"]),
                                    usuarioId.ToString()));

                            ddlEstudiante.SelectedIndex = 0;

                            // IMPEDIR CAMBIAR DE ESTUDIANTE
                            ddlEstudiante.Enabled = false;

                            return;
                        }
                    }

                    MostrarMensaje(
                        "No estás inscrito en esta actividad.");
                    return;
                }

                // DOCENTE: MOSTRAR INSCRITOS
                if (estudiantes.Rows.Count == 0)
                {
                    MostrarMensaje(
                        "Esta actividad no tiene estudiantes " +
                        "con inscripciones activas.");
                    return;
                }

                ddlEstudiante.DataSource = estudiantes;
                ddlEstudiante.DataTextField = "Nombre";
                ddlEstudiante.DataValueField = "UsuarioId";

                ddlEstudiante.DataBind();

                ddlEstudiante.Items.Insert(
                    0,
                    new ListItem(
                        "-- Seleccione un estudiante --",
                        "0"));

                ddlEstudiante.Enabled = true;
            }
            catch (Exception ex)
            {
                LimpiarListaEstudiantes();

                MostrarMensaje(
                    "Error al cargar estudiantes: " +
                    ex.Message);
            }
        }

        // LIMPIAR LISTA DE ESTUDIANTES
        private void LimpiarListaEstudiantes()
        {
            ddlEstudiante.Items.Clear();

            ddlEstudiante.Items.Add(
                new ListItem(
                    "-- Seleccione una actividad primero --",
                    "0"));

            ddlEstudiante.Enabled = EsDocente;
        }

        // REGISTRAR RESENA
        protected void btnGuardarResena_Click(
            object sender,
            EventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            if (ResenaSeleccionadaId > 0)
            {
                MostrarMensaje(
                    "Primero debe guardar o cancelar " +
                    "la modificación actual.");
                return;
            }

            int actividadId;
            int usuarioId;
            byte calificacion;

            // VALIDAR ACTIVIDAD
            if (!int.TryParse(
                ddlActividad.SelectedValue,
                out actividadId) || actividadId <= 0)
            {
                MostrarMensaje(
                    "Debe seleccionar una actividad.");
                return;
            }

            if (ddlActividad.Items.FindByValue(
                actividadId.ToString()) == null)
            {
                MostrarMensaje(
                    "La actividad seleccionada no es válida.");
                return;
            }

            // ESTUDIANTE: USAR SIEMPRE SU ID DE SESION
            if (EsEstudiante)
            {
                usuarioId = UsuarioActualId;
            }
            else
            {
                // DOCENTE: SELECCIONAR ESTUDIANTE
                if (!int.TryParse(
                    ddlEstudiante.SelectedValue,
                    out usuarioId) || usuarioId <= 0)
                {
                    MostrarMensaje(
                        "Debe seleccionar un estudiante.");
                    return;
                }
            }

            try
            {
                // VALIDAR INSCRIPCION EN EL SERVIDOR
                if (!EstaInscrito(
                    actividadId,
                    usuarioId))
                {
                    MostrarMensaje(
                        "El estudiante no tiene una " +
                        "inscripción activa en esta actividad.");
                    return;
                }

                // VALIDAR CALIFICACION
                if (!byte.TryParse(
                    ddlCalificacion.SelectedValue,
                    out calificacion) ||
                    calificacion < 1 ||
                    calificacion > 5)
                {
                    MostrarMensaje(
                        "Debe seleccionar una calificación " +
                        "de 1 a 5.");
                    return;
                }

                // CREAR RESENA
                Resena resena = new Resena();

                resena.ActividadId = actividadId;
                resena.UsuarioId = usuarioId;
                resena.Calificacion = calificacion;
                resena.Comentario =
                    txtComentario.Text.Trim();

                // GUARDAR EN SQL SERVER
                bool guardado =
                    presenter.GuardarResena(resena);

                if (guardado)
                {
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "Error al registrar la reseña: " +
                    ex.Message);
            }
        }

        // ACCIONES DE LA TABLA
        protected void gvResenas_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            int resenaId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out resenaId) || resenaId <= 0)
            {
                MostrarMensaje(
                    "No se pudo identificar la reseña.");
                return;
            }

            if (e.CommandName == "SeleccionarResena")
            {
                SeleccionarResena(resenaId);
            }
            else if (e.CommandName == "EliminarResena")
            {
                EliminarResenaSeleccionada(resenaId);
            }
        }

        // BUSCAR RESENA POR ID
        private Resena BuscarResena(int resenaId)
        {
            ResenaService servicio =
                new ResenaService();

            List<Resena> resenas =
                servicio.ObtenerResenas();

            foreach (Resena resena in resenas)
            {
                if (resena.ResenaId == resenaId)
                {
                    return resena;
                }
            }

            return null;
        }

        // VERIFICAR PERMISOS SOBRE LA RESENA
        private bool PuedeAdministrarResena(
            Resena resena)
        {
            if (resena == null)
            {
                return false;
            }

            if (EsDocente)
            {
                return true;
            }

            return EsEstudiante &&
                   resena.UsuarioId == UsuarioActualId;
        }

        // SELECCIONAR RESENA PARA MODIFICAR
        private void SeleccionarResena(int resenaId)
        {
            try
            {
                Resena seleccionada =
                    BuscarResena(resenaId);

                if (seleccionada == null)
                {
                    MostrarMensaje(
                        "La reseña seleccionada no existe.");
                    return;
                }

                if (!PuedeAdministrarResena(seleccionada))
                {
                    MostrarMensaje(
                        "No tienes permiso para modificar " +
                        "esta reseña.");
                    return;
                }

                if (EsEstudiante &&
                    !EstaInscrito(
                        seleccionada.ActividadId,
                        UsuarioActualId))
                {
                    MostrarMensaje(
                        "No puedes modificar esta reseña " +
                        "porque ya no tienes una " +
                        "inscripción activa.");
                    return;
                }

                ListItem actividad =
                    ddlActividad.Items.FindByValue(
                        seleccionada.ActividadId.ToString());

                if (actividad == null)
                {
                    MostrarMensaje(
                        "La actividad de esta reseña " +
                        "no está disponible.");
                    return;
                }

                ResenaSeleccionadaId =
                    seleccionada.ResenaId;

                ddlActividad.ClearSelection();
                actividad.Selected = true;

                CargarEstudiantesInscritos();

                ListItem estudiante =
                    ddlEstudiante.Items.FindByValue(
                        seleccionada.UsuarioId.ToString());

                // PARA EL DOCENTE, MOSTRAR EL ESTUDIANTE
                // ORIGINAL SI YA NO ESTA INSCRITO
                if (estudiante == null && EsDocente)
                {
                    estudiante = new ListItem(
                        seleccionada.NombreEstudiante,
                        seleccionada.UsuarioId.ToString());

                    ddlEstudiante.Items.Add(estudiante);
                }

                if (estudiante == null)
                {
                    throw new Exception(
                        "No se pudo identificar al estudiante.");
                }

                ddlEstudiante.ClearSelection();
                estudiante.Selected = true;

                ListItem calificacion =
                    ddlCalificacion.Items.FindByValue(
                        seleccionada.Calificacion.ToString());

                if (calificacion != null)
                {
                    ddlCalificacion.ClearSelection();
                    calificacion.Selected = true;
                }

                txtComentario.Text =
                    seleccionada.Comentario;

                ddlActividad.Enabled = false;
                ddlEstudiante.Enabled = false;

                btnGuardarResena.Visible = false;
                btnModificarResena.Visible = true;
                btnCancelarEdicion.Visible = true;

                MostrarMensaje(
                    "Editando reseña N.º " +
                    seleccionada.ResenaId +
                    ". Puede modificar la calificación " +
                    "y el comentario.");
            }
            catch (Exception ex)
            {
                LimpiarFormulario();

                MostrarMensaje(
                    "Error al seleccionar reseña: " +
                    ex.Message);
            }
        }

        // GUARDAR MODIFICACION
        protected void btnModificarResena_Click(
            object sender,
            EventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            if (ResenaSeleccionadaId <= 0)
            {
                MostrarMensaje(
                    "Primero debe seleccionar una reseña.");
                return;
            }

            try
            {
                // COMPROBAR QUIEN ES EL PROPIETARIO
                Resena original =
                    BuscarResena(ResenaSeleccionadaId);

                if (!PuedeAdministrarResena(original))
                {
                    MostrarMensaje(
                        "No tienes permiso para modificar " +
                        "esta reseña.");
                    return;
                }

                if (EsEstudiante &&
                    !EstaInscrito(
                        original.ActividadId,
                        UsuarioActualId))
                {
                    MostrarMensaje(
                        "No tienes una inscripción activa " +
                        "en esta actividad.");
                    return;
                }

                byte calificacion;

                if (!byte.TryParse(
                    ddlCalificacion.SelectedValue,
                    out calificacion) ||
                    calificacion < 1 ||
                    calificacion > 5)
                {
                    MostrarMensaje(
                        "Debe seleccionar una calificación " +
                        "de 1 a 5.");
                    return;
                }

                Resena resena = new Resena();

                resena.ResenaId = ResenaSeleccionadaId;
                resena.Calificacion = calificacion;
                resena.Comentario =
                    txtComentario.Text.Trim();

                bool modificado =
                    presenter.ModificarResena(resena);

                if (modificado)
                {
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "Error al modificar reseña: " +
                    ex.Message);
            }
        }

        // ELIMINAR RESENA
        private void EliminarResenaSeleccionada(
            int resenaId)
        {
            try
            {
                Resena original =
                    BuscarResena(resenaId);

                if (!PuedeAdministrarResena(original))
                {
                    MostrarMensaje(
                        "No tienes permiso para eliminar " +
                        "esta reseña.");
                    return;
                }

                bool eliminada =
                    presenter.EliminarResena(resenaId);

                if (eliminada)
                {
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "Error al eliminar reseña: " +
                    ex.Message);
            }
        }

        // CANCELAR MODIFICACION
        protected void btnCancelarEdicion_Click(
            object sender,
            EventArgs e)
        {
            if (!UsuarioAutorizado())
            {
                return;
            }

            LimpiarFormulario();

            MostrarMensaje(
                "Modificación cancelada.");
        }

        // LIMPIAR FORMULARIO
        private void LimpiarFormulario()
        {
            ResenaSeleccionadaId = 0;

            ddlActividad.Enabled = true;

            if (ddlActividad.Items.Count > 0)
            {
                ddlActividad.SelectedIndex = 0;
            }

            LimpiarListaEstudiantes();

            if (ddlCalificacion.Items.Count > 0)
            {
                ddlCalificacion.SelectedIndex = 0;
            }

            txtComentario.Text = "";

            btnGuardarResena.Visible = true;
            btnModificarResena.Visible = false;
            btnCancelarEdicion.Visible = false;
        }

        // MOSTRAR RESENAS EN LA TABLA
        public void MostrarResenas(
            List<Resena> resenas)
        {
            if (EsDocente)
            {
                // DOCENTE: TODAS LAS RESENAS
                gvResenas.DataSource = resenas;
            }
            else
            {
                // ESTUDIANTE: SOLO LAS PROPIAS
                List<Resena> propias =
                    new List<Resena>();

                if (resenas != null)
                {
                    foreach (Resena resena in resenas)
                    {
                        if (resena.UsuarioId ==
                            UsuarioActualId)
                        {
                            propias.Add(resena);
                        }
                    }
                }

                gvResenas.DataSource = propias;
            }

            gvResenas.DataBind();
        }

        // MOSTRAR MENSAJES
        public void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text = mensaje;
        }
    }
}
