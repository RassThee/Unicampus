
using System.Data.SqlClient;

namespace UniCampus.DAL
{
    public class ConexionBD
    {
        private string cadenaConexion =
            @"Data Source=DESKTOP-95JIR85\SQLEXPRESS;
              Initial Catalog=UniCampusDB;
              Integrated Security=True";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}
