
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniCampus.BLL;

namespace UniCampus.Web
{
    public partial class Estudiantes : Page
    {
        private UsuarioService servicio =
            new UsuarioService();

        // GUARDAR EL ID DEL ESTUDIANTE QUE SE MODIFICA
        private int EstudianteIdEdicion
        {
            get
            {
                object valor = ViewState["EstudianteIdEdicion"];

                if (valor == null)
                {
                    return 0;
                }

                return Convert.ToInt32(valor);
            }
            set
            {
                ViewState["EstudianteIdEdicion"] = value;
            }
        }

        // GUARDAR EL ID PARA RESTABLECER CONTRASEÑA
        private int EstudianteIdPassword
        {
            get
            {
                object valor = ViewState["EstudianteIdPassword"];

                if (valor == null)
                {
                    return 0;
                }

                return Convert.ToInt32(valor);
            }
            set
            {
                ViewState["EstudianteIdPassword"] = value;
            }
        }

        // VERIFICAR SI EL USUARIO ES DOCENTE
        private bool EsDocente()
        {
            int usuarioId;

            return Request.IsAuthenticated &&
                   int.TryParse(
                       Convert.ToString(Session["UsuarioId"]),
                       out usuarioId) &&
                   usuarioId > 0 &&
                   Convert.ToString(Session["RolUsuario"]) ==
                   "Organizador";
        }

        // CARGAR PAGINA
        protected void Page_Load(object sender, EventArgs e)
        {
            // SOLO LOS DOCENTES PUEDEN ENTRAR
            if (!EsDocente())
            {
                Response.Redirect(
                    "~/Default.aspx", false);

                Context.ApplicationInstance.CompleteRequest();

                return;
            }

            if (!IsPostBack)
            {
                EstudianteIdEdicion = 0;
                EstudianteIdPassword = 0;

                pnlPasswordInicial.Visible = true;
                pnlRestablecerPassword.Visible = false;

                btnRegistrar.Text = "Registrar estudiante";
                btnCancelar.Visible = false;

                CargarEstudiantes();
            }
        }

        // CONSULTAR ESTUDIANTES
        private void CargarEstudiantes()
        {
            try
            {
                gvEstudiantes.DataSource =
                    servicio.ObtenerEstudiantes();

                gvEstudiantes.DataBind();
            }
            catch (Exception)
            {
                MostrarMensaje(
                    "No se pudo cargar la lista de estudiantes.",
                    false);
            }
        }

        // REGISTRAR O MODIFICAR ESTUDIANTE
        protected void btnRegistrar_Click(
            object sender,
            EventArgs e)
        {
            if (!EsDocente())
            {
                return;
            }

            try
            {
                string nombre = txtNombre.Text.Trim();
                string correo = txtCorreo.Text.Trim();
                string programa = txtPrograma.Text.Trim();

                if (EstudianteIdEdicion == 0)
                {
                    // REGISTRAR ESTUDIANTE NUEVO
                    string password = txtPassword.Text;
                    string confirmar = txtConfirmarPassword.Text;

                    if (string.IsNullOrEmpty(password))
                    {
                        MostrarMensaje(
                            "Debe ingresar una contraseña inicial.",
                            false);
                        return;
                    }

                    if (password != confirmar)
                    {
                        MostrarMensaje(
                            "Las contraseñas no coinciden.",
                            false);
                        return;
                    }

                    servicio.RegistrarEstudiante(
                        nombre,
                        correo,
                        programa,
                        password);

                    LimpiarFormulario();
                    CargarEstudiantes();

                    MostrarMensaje(
                        "Estudiante registrado correctamente. " +
                        "Ya puede iniciar sesión con su correo " +
                        "y contraseña.",
                        true);
                }
                else
                {
                    // MODIFICAR ESTUDIANTE EXISTENTE
                    servicio.ModificarEstudiante(
                        EstudianteIdEdicion,
                        nombre,
                        correo,
                        programa);

                    LimpiarFormulario();
                    CargarEstudiantes();

                    MostrarMensaje(
                        "Estudiante modificado correctamente.",
                        true);
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    MostrarMensaje(
                        "No se pudo guardar el estudiante " +
                        "en la base de datos.",
                        false);
                }
                else
                {
                    MostrarMensaje(ex.Message, false);
                }
            }
        }

        // ACCIONES DE LA TABLA
        protected void gvEstudiantes_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (!EsDocente())
            {
                return;
            }

            int usuarioId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out usuarioId) || usuarioId <= 0)
            {
                MostrarMensaje(
                    "El ID del estudiante no es válido.",
                    false);
                return;
            }

            try
            {
                // MODIFICAR ESTUDIANTE
                if (e.CommandName == "ModificarEstudiante")
                {
                    DataRow estudiante =
                        servicio.ObtenerEstudiantePorId(usuarioId);

                    txtNombre.Text =
                        Convert.ToString(estudiante["Nombre"]);

                    txtCorreo.Text =
                        Convert.ToString(estudiante["Correo"]);

                    txtPrograma.Text =
                        Convert.ToString(estudiante["Programa"]);

                    EstudianteIdEdicion = usuarioId;

                    btnRegistrar.Text = "Guardar cambios";
                    btnCancelar.Visible = true;

                    // NO CAMBIAR CONTRASEÑA AL MODIFICAR DATOS
                    pnlPasswordInicial.Visible = false;

                    MostrarMensaje(
                        "Modifique los datos del estudiante " +
                        "y presione Guardar cambios.",
                        true);
                }

                // DESACTIVAR ESTUDIANTE
                else if (e.CommandName == "DesactivarEstudiante")
                {
                    servicio.DesactivarEstudiante(usuarioId);

                    if (EstudianteIdEdicion == usuarioId)
                    {
                        LimpiarFormulario();
                    }

                    if (EstudianteIdPassword == usuarioId)
                    {
                        LimpiarPanelPassword();
                    }

                    CargarEstudiantes();

                    MostrarMensaje(
                        "Estudiante desactivado correctamente.",
                        true);
                }

                // REACTIVAR ESTUDIANTE
                else if (e.CommandName == "ReactivarEstudiante")
                {
                    servicio.ReactivarEstudiante(usuarioId);

                    CargarEstudiantes();

                    MostrarMensaje(
                        "Estudiante reactivado correctamente.",
                        true);
                }

                // SELECCIONAR ESTUDIANTE PARA CAMBIAR CONTRASEÑA
                else if (e.CommandName == "RestablecerPassword")
                {
                    DataRow estudiante =
                        servicio.ObtenerEstudiantePorId(usuarioId);

                    EstudianteIdPassword = usuarioId;

                    lblEstudiantePassword.Text =
                        Server.HtmlEncode(
                            Convert.ToString(estudiante["Nombre"]));

                    txtNuevaPassword.Text = "";
                    txtConfirmarNuevaPassword.Text = "";
                    lblMensajePassword.Text = "";

                    pnlRestablecerPassword.Visible = true;

                    MostrarMensaje(
                        "Escriba la nueva contraseña " +
                        "en el panel de restablecimiento.",
                        true);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        // GUARDAR NUEVA CONTRASEÑA
        protected void btnGuardarPassword_Click(
            object sender,
            EventArgs e)
        {
            if (!EsDocente())
            {
                return;
            }

            try
            {
                int usuarioId = EstudianteIdPassword;

                if (usuarioId <= 0)
                {
                    MostrarMensajePassword(
                        "Debe seleccionar un estudiante.",
                        false);
                    return;
                }

                string nuevaPassword =
                    txtNuevaPassword.Text;

                string confirmar =
                    txtConfirmarNuevaPassword.Text;

                if (string.IsNullOrEmpty(nuevaPassword))
                {
                    MostrarMensajePassword(
                        "Debe ingresar la nueva contraseña.",
                        false);
                    return;
                }

                if (nuevaPassword != confirmar)
                {
                    MostrarMensajePassword(
                        "Las contraseñas no coinciden.",
                        false);
                    return;
                }

                servicio.RestablecerPassword(
                    usuarioId,
                    nuevaPassword);

                LimpiarPanelPassword();

                MostrarMensaje(
                    "Contraseña restablecida correctamente. " +
                    "El estudiante ya puede iniciar sesión " +
                    "con la nueva contraseña.",
                    true);
            }
            catch (Exception ex)
            {
                MostrarMensajePassword(
                    ex.Message,
                    false);
            }
        }

        // CANCELAR RESTABLECIMIENTO
        protected void btnCancelarPassword_Click(
            object sender,
            EventArgs e)
        {
            if (!EsDocente())
            {
                return;
            }

            LimpiarPanelPassword();

            MostrarMensaje(
                "Restablecimiento de contraseña cancelado.",
                true);
        }

        // CANCELAR MODIFICACION
        protected void btnCancelar_Click(
            object sender,
            EventArgs e)
        {
            if (!EsDocente())
            {
                return;
            }

            LimpiarFormulario();

            MostrarMensaje(
                "Modificación cancelada.",
                true);
        }

        // LIMPIAR FORMULARIO
        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtPrograma.Text = "";

            txtPassword.Text = "";
            txtConfirmarPassword.Text = "";

            EstudianteIdEdicion = 0;

            btnRegistrar.Text = "Registrar estudiante";
            btnCancelar.Visible = false;

            pnlPasswordInicial.Visible = true;
        }

        // LIMPIAR PANEL DE CONTRASEÑA
        private void LimpiarPanelPassword()
        {
            EstudianteIdPassword = 0;

            txtNuevaPassword.Text = "";
            txtConfirmarNuevaPassword.Text = "";

            lblEstudiantePassword.Text = "";
            lblMensajePassword.Text = "";

            pnlRestablecerPassword.Visible = false;
        }

        // MOSTRAR MENSAJES GENERALES
        private void MostrarMensaje(
            string mensaje,
            bool correcto)
        {
            lblMensaje.Text = mensaje;

            if (correcto)
            {
                lblMensaje.ForeColor =
                    System.Drawing.Color.Green;
            }
            else
            {
                lblMensaje.ForeColor =
                    System.Drawing.Color.Red;
            }
        }

        // MOSTRAR MENSAJES DE CONTRASEÑA
        private void MostrarMensajePassword(
            string mensaje,
            bool correcto)
        {
            lblMensajePassword.Text = mensaje;

            if (correcto)
            {
                lblMensajePassword.ForeColor =
                    System.Drawing.Color.Green;
            }
            else
            {
                lblMensajePassword.ForeColor =
                    System.Drawing.Color.Red;
            }
        }
    }
}
