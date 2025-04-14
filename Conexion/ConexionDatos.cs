using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Conexion
{
    public class ConexionDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector
        {
            get {  return lector; }
        }
        public ConexionDatos()
        {
            conexion = new SqlConnection("server=DESKTOP-UMPK4FH\\SQLEXPRESS; database=CATALOGO_DB; integrated security= true");
            comando = new SqlCommand();
        }
        public void Consulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }
        public void Lectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EjecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Parametro(string nom, object valor)
        {
            comando.Parameters.AddWithValue(nom, valor);
        }
        public void CerrarCenexion()
        {
            if(lector != null)
                lector.Close();
            conexion.Close();
        }
    }
}
