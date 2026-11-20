
using System;
using System.Web;
using System.Web.UI;

namespace UniCampus.Web
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar que el usuario haya iniciado sesión.
            if (!Request.IsAuthenticated ||
                Session["UsuarioId"] == null ||
                Session["NombreUsuario"] == null ||
                Session["RolUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Obtener los datos del usuario conectado.
            string nombre = Convert.ToString(
                Session["NombreUsuario"]);

            string rol = Convert.ToString(
                Session["RolUsuario"]);

            string nombreRol = "";

            // Cambiar el nombre visible del rol.
            if (rol == "Organizador")
            {
                nombreRol = "Docente";
            }
            else if (rol == "Participante")
            {
                nombreRol = "Estudiante";
            }
            else
            {
                Response.Redirect("~/CerrarSesion.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Mostrar el nombre del usuario.
            lblNombreUsuario.Text =
                "Bienvenido, " +
                Server.HtmlEncode(nombre);

            // Mostrar el rol con el nuevo nombre.
            lblRolUsuario.Text =
                "Rol: " +
                Server.HtmlEncode(nombreRol);
        }
    }
}
