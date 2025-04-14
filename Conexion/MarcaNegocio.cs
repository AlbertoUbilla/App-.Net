using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Conexion
{
    public class MarcaNegocio
    {
        public List<Marca> listar()
        {
            List<Marca> listaMarca = new List<Marca>();
            ConexionDatos datos = new ConexionDatos();
            try
            {
                datos.Consulta("select Id, Descripcion from Marcas");
                datos.Lectura();
                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    listaMarca.Add(aux);
                }
                return listaMarca;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
            finally { datos.CerrarCenexion(); }
        }
    }
}
