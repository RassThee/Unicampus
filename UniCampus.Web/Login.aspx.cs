
using System;
using System.Data;
using System.Diagnostics;
using System.Web.Security;
using System.Web.UI;
using UniCampus.BLL;

namespace UniCampus.Web
{
    public partial class Login : Page
    {
        private AutenticacionService servicio =
            new AutenticacionService();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Comprobar que la autenticación y la sesión
            // correspondan a un usuario válido.
            if (!IsPostBack && Request.IsAuthenticated)
            {
                bool sesionValida =
                    Session["UsuarioId"] != null &&
                    Session["NombreUsuario"] != null &&
                    Session["RolUsuario"] != null;

                if (sesionValida)
                {
                    Response.Redirect(
                        "~/Default.aspx", false);

                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                // Existe una cookie, pero no una sesión válida.
                FormsAuthentication.SignOut();

                Session.Clear();
                Session.Abandon();
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";

            string correo = txtCorreo.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrEmpty(password))
            {
                MostrarMensaje(
                    "Debe ingresar su correo y contraseña.");
                return;
            }

            try
            {
                // Validar las credenciales en SQL Server.
                DataRow usuario =
                    servicio.IniciarSesion(correo, password);

                if (usuario == null)
                {
                    MostrarMensaje(
                        "Correo o contraseña incorrectos.");
                    return;
                }

                int usuarioId =
                    Convert.ToInt32(usuario["UsuarioId"]);

                string nombre =
                    Convert.ToString(usuario["Nombre"]);

                string rol =
                    Convert.ToString(usuario["Rol"]);

                // Guardar la información del usuario.
                Session["UsuarioId"] = usuarioId;
                Session["NombreUsuario"] = nombre;
                Session["RolUsuario"] = rol;

                // Crear la cookie de autenticación.
                FormsAuthentication.SetAuthCookie(
                    correo, false);

                // Ir a la página principal.
                Response.Redirect(
                    "~/Default.aspx", false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Registrar el error real en Visual Studio.
                Debug.WriteLine(
                    "ERROR AL INICIAR SESION EN UNICAMPUS:");

                Debug.WriteLine(ex.ToString());

                // Mostrar el detalle solamente en depuración.
#if DEBUG
                MostrarMensaje(
                    "Error: " + ex.Message);
#else
                MostrarMensaje(
                    "No fue posible iniciar sesión. " +
                    "Intente nuevamente.");
#endif
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text =
                Server.HtmlEncode(mensaje);

            lblMensaje.ForeColor =
                System.Drawing.Color.Red;
        }
    }
}
