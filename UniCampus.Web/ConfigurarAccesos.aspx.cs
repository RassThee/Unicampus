
using System;
using System.Drawing;
using UniCampus.BLL;

namespace UniCampus.Web
{
    public partial class ConfigurarAccesos : System.Web.UI.Page
    {
        private ConfiguracionInicialService servicio =
            new ConfiguracionInicialService();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Esta página es solo para configurar
            // las contraseñas iniciales de prueba.
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";

            int usuarioId;

            // Validar que se seleccione un usuario.
            if (!int.TryParse(ddlUsuarios.SelectedValue, out usuarioId)
                || usuarioId <= 0)
            {
                MostrarMensaje(
                    "Seleccione un usuario.",
                    Color.Red);
                return;
            }

            string password = txtPassword.Text;
            string confirmarPassword = txtConfirmarPassword.Text;

            // Validar que se escriba una contraseña.
            if (string.IsNullOrWhiteSpace(password))
            {
                MostrarMensaje(
                    "Ingrese una contraseña.",
                    Color.Red);
                return;
            }

            // Mínimo 5 caracteres para las pruebas.
            if (password.Length < 5)
            {
                MostrarMensaje(
                    "La contraseña debe tener al menos 5 caracteres.",
                    Color.Red);
                return;
            }

            // Verificar que ambas contraseñas coincidan.
            if (password != confirmarPassword)
            {
                MostrarMensaje(
                    "Las contraseñas no coinciden.",
                    Color.Red);
                return;
            }

            try
            {
                bool guardado =
                    servicio.ConfigurarPasswordInicial(
                        usuarioId, password);

                if (guardado)
                {
                    MostrarMensaje(
                        "Contraseña configurada correctamente.",
                        Color.Green);

                    ddlUsuarios.SelectedIndex = 0;
                }
                else
                {
                    MostrarMensaje(
                        "No se pudo configurar la contraseña. " +
                        "Es posible que el usuario ya tenga una asignada.",
                        Color.Red);
                }
            }
            catch (Exception)
            {
                MostrarMensaje(
                    "Ocurrió un error al guardar la contraseña. " +
                    "Verifique la conexión con la base de datos.",
                    Color.Red);
            }
        }

        private void MostrarMensaje(string mensaje, Color color)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = color;
        }
    }
}
