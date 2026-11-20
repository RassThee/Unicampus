
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace UniCampus.Web
{
    public partial class CerrarSesion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cerrar la autenticación del usuario.
            FormsAuthentication.SignOut();

            // Eliminar los datos guardados en la sesión.
            Session.Clear();
            Session.Abandon();

            // Evitar que esta respuesta quede en caché.
            Response.Cache.SetCacheability(
                HttpCacheability.NoCache);

            Response.Cache.SetNoStore();

            // Regresar a la pantalla de inicio de sesión.
            Response.Redirect("~/Login.aspx", false);

            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
